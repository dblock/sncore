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
                mAccountFeed = SessionManager.GetCachedItem<TransitAccountFeed>(SyndicationService, "GetAccountFeedById", args);
            }

            return mAccountFeed;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                if (FeedId > 0)
                {
                    TransitAccountFeed f = Feed;

                    TransitFeedType t = SyndicationService.GetFeedTypeByName(f.FeedType);
                    gridManage.RepeatColumns = t.SpanColumnsPreview;
                    gridManage.RepeatRows = t.SpanRowsPreview;
                    object[] args = { FeedId };
                    gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
                        SyndicationService, "GetAccountFeedItemsCountById", args);
                    gridManage_OnGetDataSource(this, null);
                    gridManage.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            object[] args = { FeedId, options };
            gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountFeedItem>(
                SyndicationService, "GetAccountFeedItemsById", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
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
            return Renderer.CleanHtml(description.ToString(), feedlinkUri);
        }
    }
}
