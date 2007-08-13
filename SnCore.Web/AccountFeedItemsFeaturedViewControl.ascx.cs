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
using SnCore.Services;
using System.Collections.Generic;
using SnCore.Tools.Web;
using SnCore.Tools.Web.Html;
using SnCore.WebServices;

public partial class AccountFeedItemsFeaturedViewControl : Control
{
    private int mItemsCollapseAfter = 3;
    private int mItemsShown = 0;
    private int mImagesShown = 0;

    public int ItemsCollapseAfter
    {
        get
        {
            return mItemsCollapseAfter;
        }
        set
        {
            mItemsCollapseAfter = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            gridManage.VirtualItemCount = Math.Min(gridManage.PageSize, SessionManager.GetCount<TransitFeature, string>(
                "AccountFeedItem", SessionManager.ObjectService.GetFeaturesCount));
            gridManage_OnGetDataSource(this, null);
            gridManage.DataBind();
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

    public string GetTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            return "Untitled";

        return Renderer.Render(Renderer.RemoveHtml(title));
    }

    public string GetDescription(string description)
    {
        if (mItemsShown++ >= mItemsCollapseAfter)
            return string.Empty;

        if (string.IsNullOrEmpty(description))
            return string.Empty;

        return base.GetSummary(description);
    }

    public string GetImage(string description, string root)
    {
        if (mImagesShown++ >= mItemsCollapseAfter)
            return string.Empty;

        if (string.IsNullOrEmpty(description))
            return string.Empty;

        List<HtmlImage> list = HtmlImageExtractor.Extract(description);

        if (list.Count == 0)
            return string.Empty;

        Uri rooturi = null;
        Uri uri = null;
        Uri.TryCreate(root, UriKind.Absolute, out rooturi);
        Uri.TryCreate(rooturi, list[0].Src, out uri);
        Uri imgrewriteuri = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src=");

        return string.Format("<img class='sncore_blog_image' border='0' src='{0}{1}'>", 
            imgrewriteuri, Renderer.UrlEncode(uri.ToString()));
    }

    public string GetComments(int count)
    {
        if (count == 0) return "post a comment";
        else if (count == 1) return "1 comment";
        else return string.Format("{0} comments", count);
    }

    public TransitAccountFeedItem GetAccountFeedItem(int id)
    {
        return SessionManager.GetInstance<TransitAccountFeedItem, int>(
            id, SessionManager.SyndicationService.GetAccountFeedItemById);
    }
}
