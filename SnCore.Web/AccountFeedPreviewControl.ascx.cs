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
using Wilco.Web.UI;
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;

public partial class AccountFeedPreviewControl : Control
{
    private TransitAccountFeed mAccountFeed = null;

    public bool ShowTitle
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "ShowTitle", true);
        }
        set
        {
            ViewState["ShowTitle"] = value;
        }
    }

    public bool LinkDescription
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "LinkDescription", false);
        }
        set
        {
            ViewState["LinkDescription"] = value;
        }
    }

    public int FeedId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "FeedId", 0);
        }
        set
        {
            ViewState["FeedId"] = value;
        }
    }

    public TransitAccountFeed Feed
    {
        get
        {
            if (mAccountFeed == null)
            {
                object[] args = { SessionManager.IsLoggedIn ? SessionManager.Ticket : string.Empty, FeedId };
                mAccountFeed = SessionManager.GetCachedItem<TransitAccountFeed>(
                    SessionManager.SyndicationService, "GetAccountFeedById", args);
            }

            return mAccountFeed;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            if (FeedId > 0)
            {
                TransitAccountFeed f = Feed;

                TransitFeedType t = SessionManager.SyndicationService.GetFeedTypeByName(SessionManager.Ticket, f.FeedType);
                gridManage.RepeatColumns = t.SpanColumnsPreview;
                gridManage.RepeatRows = t.SpanRowsPreview;
                object[] args = { SessionManager.Ticket, FeedId };
                gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount<TransitAccountFeedItem>(
                    SessionManager.SyndicationService, "GetAccountFeedItemsCount", args);
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();
            }
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        object[] args = { SessionManager.Ticket, FeedId, options };
        gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountFeedItem>(
            SessionManager.SyndicationService, "GetAccountFeedItems", args);
    }

    public string GetTitle(object title)
    {
        if (!ShowTitle)
            return string.Empty;

        return Renderer.Render(Renderer.RemoveHtml(title));
    }

    public string GetDescription(object link, object description)
    {
        if (LinkDescription)
        {
            return string.Format("<a href='{0}' target='_blank'>{1}</a>",
                link, RenderEx(description));
        }
        else
        {
            Uri feedlinkUri = null;
            Uri.TryCreate(Feed.LinkUrl, UriKind.Absolute, out feedlinkUri);
            Uri imgrewriteuri = new Uri(SessionManager.WebsiteUrl.TrimEnd("/".ToCharArray()) + "/AccountFeedItemPicture.aspx?Src={url}", UriKind.Absolute);
            return Renderer.CleanHtml(description.ToString(), feedlinkUri, imgrewriteuri);
        }
    }
}
