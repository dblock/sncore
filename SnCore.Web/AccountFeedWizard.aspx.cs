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
        if (feedtypes == null)
            throw new Exception("No feed types registered.");

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

    public void discover_Click(object sender, EventArgs e)
    {
        try
        {
            if (!Uri.IsWellFormedUriString(inputLinkUrl.Text, UriKind.Absolute))
                inputLinkUrl.Text = "http://" + inputLinkUrl.Text;

            SimpleFetcher fetcher = new SimpleFetcher();
            FetchResponse response = fetcher.Get(new Uri(inputLinkUrl.Text));
            NameValueCollection[] results = LinkParser.ParseLinkAttrs(response.data, response.length, response.charset);

            ArrayList feeds = new ArrayList();

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
                        feed.Name = title;
                        feeds.Add(feed);
                        break;
                }
            }

            if (feeds.Count == 0)
            {
                throw new Exception("No feeds found. Make sure the given address points to a page with " + 
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
        link
    };

    public void gridFeeds_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Next":
                    string name = e.Item.Cells[(int) Cells.name].Text;
                    string feed = e.Item.Cells[(int) Cells.feed].Text;
                    string link = e.Item.Cells[(int) Cells.link].Text;
                    Redirect(string.Format("AccountFeedEdit.aspx?name={0}&feed={1}&link={2}&type={3}",
                        Renderer.UrlEncode(name),
                        Renderer.UrlEncode(feed),
                        Renderer.UrlEncode(link),
                        Renderer.UrlEncode(GetDefaultFeedType().Name)));
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
