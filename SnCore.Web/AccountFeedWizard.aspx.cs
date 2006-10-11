using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Wilco.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Tools.Web;
using SnCore.WebServices;
using SnCore.Services;
using Janrain.OpenId;
using Janrain.OpenId.Consumer;
using System.Collections.Specialized;
using Rss;
using Atom.Core;
using System.Net;

public partial class AccountFeedWizard : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(linkDiscover);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public TransitFeedType GetDefaultFeedType()
    {
        List<TransitFeedType> feedtypes = SyndicationService.GetFeedTypes();
        
        if (feedtypes == null || feedtypes.Count == 0)
        {
            throw new Exception("No feed types registered.");
        }

        TransitFeedType feedtype = feedtypes[0];

        for (int i = 0; i < feedtypes.Count; i++)
        {
            if (feedtypes[i].Name.Contains("RSS"))
            {
                feedtype = feedtypes[i];
                break;
            }
        }

        return feedtype;
    }

    protected HttpWebRequest GetFeedHttpRequest(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        System.Net.ServicePointManager.Expect100Continue = false;
        request.UserAgent = "SnCore/1.0";
        request.Timeout = 60 * 1000;
        request.KeepAlive = false;
        request.MaximumAutomaticRedirections = 5;
        return request;
    }

    protected Stream GetFeedStream(string url)
    {
        return GetFeedHttpRequest(url).GetResponse().GetResponseStream();
    }

    protected void discoverRel(string url, ArrayList feeds)
    {
        SimpleFetcher fetcher = new SimpleFetcher();
        FetchResponse response = fetcher.Get(new Uri(inputLinkUrl.Text));

        NameValueCollection[] results = null;

        try
        {
            results = LinkParser.ParseLinkAttrs(response.data, response.length, response.charset);
        }
        catch
        {
            return;
        }

        foreach (NameValueCollection attrs in results)
        {
            string rel = attrs["rel"];
            if (rel == null)
                continue;

            string href = attrs["href"];
            if (href == null)
                continue;

            string type = attrs["type"];
            if (type == null)
                continue;

            string title = attrs["title"];
            if (title == null)
                title = "Untitled";

            switch (type)
            {
                case "application/rss+xml":
                case "application/atom+xml":
                    TransitAccountFeed feed = new TransitAccountFeed();
                    feed.FeedUrl = new Uri(new Uri(inputLinkUrl.Text), href).ToString();
                    feed.LinkUrl = inputLinkUrl.Text;
                    feed.Description = string.Empty;
                    feed.Name = title;
                    feeds.Add(feed);
                    break;
            }
        }
    }

    protected void discoverRss(string url, ArrayList feeds)
    {
        try
        {
            RssFeed rssfeed = RssFeed.Read(GetFeedHttpRequest(url));

            foreach (RssChannel rsschannel in rssfeed.Channels)
            {
                TransitAccountFeed feed = new TransitAccountFeed();
                feed.Description = rsschannel.Description;
                feed.FeedUrl = url;
                feed.LinkUrl = rsschannel.Link.ToString();
                feed.Name = rsschannel.Title;
                feeds.Add(feed);
                break;
            }
        }
        catch
        {
        }
    }

    protected void discoverAtom(string url, ArrayList feeds, Uri ns)
    {
        try
        {
            AtomFeed atomfeed = AtomFeed.Load(GetFeedStream(url), ns);

            TransitAccountFeed feed = new TransitAccountFeed();

            if (atomfeed.SubTitle != null)
            {
                feed.Description = atomfeed.SubTitle.Content;
            }
            else if (atomfeed.Tagline != null)
            {
                feed.Description = atomfeed.Tagline.Content;
            }

            feed.FeedUrl = url;
            
            if (atomfeed.Links != null)
            {
                foreach (AtomLink link in atomfeed.Links)
                {
                    if (link.Rel == Relationship.Alternate)
                    {
                        feed.LinkUrl = link.HRef.ToString();
                        break;
                    }
                }
            }

            if (atomfeed.Title != null)
            {
                feed.Name = atomfeed.Title.Content;
            }

            feeds.Add(feed);
        }
        catch
        {
        }
    }

    public void discover_Click(object sender, EventArgs e)
    {
        try
        {
            inputLinkUrl.Text = inputLinkUrl.Text.Replace("feed://", "http://");

            if (!Uri.IsWellFormedUriString(inputLinkUrl.Text, UriKind.Absolute))
                inputLinkUrl.Text = "http://" + inputLinkUrl.Text;

            ArrayList feeds = new ArrayList();

            discoverRel(inputLinkUrl.Text, feeds);
            if (feeds.Count == 0) discoverRss(inputLinkUrl.Text, feeds);
            if (feeds.Count == 0) discoverAtom(inputLinkUrl.Text, feeds, new Uri("http://www.w3.org/2005/Atom"));
            if (feeds.Count == 0) discoverAtom(inputLinkUrl.Text, feeds, new Uri("http://purl.org/atom/ns#"));

            if (feeds.Count == 0)
            {
                throw new Exception("No feeds found. Make sure the given address points to a valid RSS/ATOM feed or a page with " + 
                    "&lt;link rel=\"alternate\" type=\"application/rss+xml\" ... &gt; field(s) in the body.");
            }

            gridFeeds.DataSource = feeds;
            gridFeeds.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0,
        name,
        feed,
        link,
        description
    };

    protected string CleanCell(string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        if (s == "&nbsp;")
            return string.Empty;

        return s.Trim();
    }

    public void gridFeeds_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Test":
                    inputLinkUrl.Text = e.Item.Cells[(int)Cells.feed].Text;
                    discover_Click(sender, e);
                    break;
                case "Choose":
                    string name = CleanCell(e.Item.Cells[(int) Cells.name].Text);
                    string feed = CleanCell(e.Item.Cells[(int) Cells.feed].Text);
                    string link = CleanCell(e.Item.Cells[(int) Cells.link].Text);
                    string description = CleanCell(e.Item.Cells[(int)Cells.description].Text);
                    Redirect(string.Format("AccountFeedEdit.aspx?name={0}&feed={1}&link={2}&type={3}&description={4}",
                        Renderer.UrlEncode(name),
                        Renderer.UrlEncode(feed),
                        Renderer.UrlEncode(link),
                        Renderer.UrlEncode(GetDefaultFeedType().Name),
                        Renderer.UrlEncode(description)));
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
