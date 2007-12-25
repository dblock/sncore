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
using SnCore.Tools.Web.Html;

public partial class AccountFeedPreviewControl : Control
{
    private TransitAccountFeed mAccountFeed = null;

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
                mAccountFeed = SessionManager.GetInstance<TransitAccountFeed, int>(
                    FeedId, SessionManager.SyndicationService.GetAccountFeedById);
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

                TransitFeedType t = SessionManager.GetInstance<TransitFeedType, string>(
                    f.FeedType, SessionManager.SyndicationService.GetFeedTypeByName);
                gridManage.RepeatColumns = t.SpanColumnsPreview;
                gridManage.RepeatRows = t.SpanRowsPreview;
                gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountFeedItem, int>(
                    FeedId, SessionManager.SyndicationService.GetAccountFeedItemsCount);
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
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountFeedItem, int>(
            FeedId, options, SessionManager.SyndicationService.GetAccountFeedItems);
    }

    public string GetTitle(object title)
    {
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
            Uri imgrewriteuri = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
            HtmlWriterOptions options = new HtmlWriterOptions();
            options.BaseHref = feedlinkUri;
            options.RewriteImgSrc = imgrewriteuri;
            return Renderer.CleanHtml(description.ToString(), options);
        }
    }
}
