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
using SnCore.Tools.Web.Html;
using SnCore.WebServices;
using SnCore.Services;
using System.Collections.Specialized;
using Rss;
using Atom.Core;
using System.Net;
using Wilco.Web.UI;
using SnCore.SiteMap;

public class TransitAccountFeedWithItems : TransitAccountFeed
{
    private List<TransitAccountFeedItem> mFeedItems = new List<TransitAccountFeedItem>();

    public List<TransitAccountFeedItem> FeedItems
    {
        get
        {
            return mFeedItems;
        }
    }

    protected static TransitAccountFeedWithItems discoverRss(string url, string useragent)
    {
        try
        {
            RssFeed rssfeed = RssFeed.Read(GetFeedHttpRequest(url, useragent));

            foreach (RssChannel rsschannel in rssfeed.Channels)
            {
                TransitAccountFeedWithItems result = new TransitAccountFeedWithItems();
                result.Description = rsschannel.Description;
                result.FeedUrl = url;
                result.LinkUrl = rsschannel.Link.ToString();
                result.Name = rsschannel.Title;

                foreach(RssItem rssitem in rsschannel.Items)
                {
                    TransitAccountFeedItem item = new TransitAccountFeedItem();
                    item.AccountFeedLinkUrl = result.LinkUrl;
                    item.AccountFeedName = result.Name;
                    item.Created = rssitem.PubDate;
                    item.Description = rssitem.Description;
                    item.Link = rssitem.Link.ToString();
                    item.Title = rssitem.Title;
                    item.Updated = DateTime.UtcNow;
                    result.FeedItems.Add(item);
                }

                return result;
            }
        }
        catch
        {

        }

        return null;
    }

    protected static TransitAccountFeedWithItems discoverAtom(string url, string useragent, Uri ns)
    {
        try
        {
            AtomFeed atomfeed = AtomFeed.Load(GetFeedStream(url, useragent), ns);

            TransitAccountFeedWithItems result = new TransitAccountFeedWithItems();

            if (atomfeed.SubTitle != null)
            {
                result.Description = atomfeed.SubTitle.Content;
            }
            else if (atomfeed.Tagline != null)
            {
                result.Description = atomfeed.Tagline.Content;
            }

            result.FeedUrl = url;

            if (atomfeed.Links != null)
            {
                foreach (AtomLink link in atomfeed.Links)
                {
                    if (link.Rel == Relationship.Alternate)
                    {
                        result.LinkUrl = link.HRef.ToString();
                        break;
                    }
                }
            }

            if (atomfeed.Title != null)
            {
                result.Name = atomfeed.Title.Content;
            }

            foreach (AtomEntry atomitem in atomfeed.Entries)
            {
                TransitAccountFeedItem item = new TransitAccountFeedItem();
                item.Description = string.Empty;
                foreach (AtomContent content in atomitem.Contents)
                {
                    if (!string.IsNullOrEmpty(item.Description))
                        item.Description += "\n";

                    switch (content.Type)
                    {
                        case MediaType.TextHtml:
                            item.Description += HttpUtility.HtmlDecode(content.Content);
                            break;
                        default:
                            item.Description += content.Content;
                            break;
                    }
                }

                if (string.IsNullOrEmpty(item.Description))
                {
                    item.Description = atomitem.Summary.Content;
                }

                item.Title = atomitem.Title.Content;

                if (atomitem.Links.Count > 0)
                {
                    foreach (AtomLink link in atomitem.Links)
                    {
                        if (link.Rel == Relationship.Alternate)
                        {
                            item.Link = link.HRef.ToString();
                        }
                    }
                }

                if (atomitem.Id != null && !string.IsNullOrEmpty(atomitem.Id.ToString())) item.Guid = atomitem.Id.ToString();
                if (atomitem.Created != null && atomitem.Created.DateTime.Ticks > 0) item.Created = atomitem.Created.DateTime.ToUniversalTime();
                result.FeedItems.Add(item);
            }

            return result;
        }
        catch
        {
            return null;
        }
    }

    public static TransitAccountFeedWithItems Discover(string url, string useragent)
    {
        TransitAccountFeedWithItems feed = null;

        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("Missing Url");
        }

        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            throw new Exception(string.Format("Invalid url: {0}", url));
        }

        feed = discoverRss(url, useragent);
        if (feed == null) feed = discoverAtom(url, useragent, new Uri("http://www.w3.org/2005/Atom"));
        if (feed == null) feed = discoverAtom(url, useragent, new Uri("http://purl.org/atom/ns#"));

        return feed;
    }

    protected static HttpWebRequest GetFeedHttpRequest(string url, string useragent)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        System.Net.ServicePointManager.Expect100Continue = false;
        request.UserAgent = useragent;
        request.Timeout = 60 * 1000;
        request.KeepAlive = false;
        request.MaximumAutomaticRedirections = 5;
        return request;
    }

    protected static Stream GetFeedStream(string url, string useragent)
    {
        return GetFeedHttpRequest(url, useragent).GetResponse().GetResponseStream();
    }
}

public partial class AccountFeedTest : Page
{
    private TransitAccountFeedWithItems mAccountFeed = null;

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string useragent = SessionManager.GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0");
            TransitAccountFeedWithItems f = TransitAccountFeedWithItems.Discover(Request["url"], useragent);

            if (f == null)
            {
                ReportInfo(string.Format("Sorry, {0} is not a valid RSS or ATOM feed.",
                    Renderer.Render(Request["url"])));
                panelAll.Visible = false;
                return;
            }

            mAccountFeed = f;

            labelFeed.Text = Renderer.Render(f.Name);
            labelFeed.NavigateUrl = f.LinkUrl;
            labelFeedDescription.Text = Renderer.Render(f.Description);

            labelAccountName.Text = Renderer.Render(SessionManager.Account.Name);
            linkAccount.HRef = string.Format("AccountView.aspx?id={0}", SessionManager.Account.Id);
            imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", SessionManager.Account.PictureId);

            this.Title = string.Format("{0}'s {1}", Renderer.Render(SessionManager.Account.Name), Renderer.Render(f.Name));

            if (f.FeedItems.Count > gridManage.PageSize)
            {
                f.FeedItems.RemoveRange(gridManage.PageSize, 
                    f.FeedItems.Count - gridManage.PageSize);
            }

            gridManage.DataSource = f.FeedItems;
            gridManage.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(f.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public string GetDescription(string value)
    {
        Uri imgrewriteuri = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
        return Renderer.CleanHtml(value,
            Uri.IsWellFormedUriString(mAccountFeed.LinkUrl, UriKind.Absolute) ? new Uri(mAccountFeed.LinkUrl) : null,
            imgrewriteuri);
    }
}
