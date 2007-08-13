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
using SnCore.Tools.Web;
using System.Text;
using SnCore.Services;
using SnCore.WebServices;
using System.Reflection;
using System.Collections.Generic;
using SnCore.SiteMap;

public partial class FeaturedAccountFeedItemsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Featured Posts", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    private void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.ObjectService.GetFeaturesCount(
            SessionManager.Ticket, "AccountFeedItem");
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();

        if (gridManage.VirtualItemCount == 0)
        {
            labelCount.Text = "No Featured Blog Posts";
        }
        else if (gridManage.VirtualItemCount == 1)
        {
            labelCount.Text = "1 Featured Blog Post";
        }
        else
        {
            labelCount.Text = string.Format("{0} Featured Blog Posts!", gridManage.VirtualItemCount);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;

        gridManage.DataSource = SessionManager.GetCollection<TransitFeature, string>(
            "AccountFeedItem", serviceoptions, SessionManager.ObjectService.GetFeatures);
    }

    public TransitAccountFeedItem GetAccountFeedItem(int id)
    {
        return SessionManager.GetInstance<TransitAccountFeedItem, int>(
            id, SessionManager.SyndicationService.GetAccountFeedItemById);
    }

    public string GetComments(int count)
    {
        if (count == 0) return "read and comment";
        else if (count == 1) return "read 1 comment";
        else return string.Format("read {0} comments", count);
    }

    public string GetSummary(string summary, string link)
    {
        Uri uri = null;
        Uri.TryCreate(link, UriKind.Absolute, out uri);
        Uri imgrewriteuri = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
        return Renderer.CleanHtml(summary, uri, imgrewriteuri);
    }
}
