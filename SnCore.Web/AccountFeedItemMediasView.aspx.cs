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
using SnCore.Services;
using SnCore.WebServices;
using Wilco.Web.UI;
using SnCore.SiteMap;

public partial class AccountFeedItemMediasView : AccountPersonPage
{
    public bool IsEditing
    {
        get
        {
            return SessionManager.IsAdministrator &&
                ViewStateUtility.GetViewStateValue<bool>(
                    ViewState, "IsEditing", false);
        }
        set
        {
            ViewState["IsEditing"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            linkEdit.Visible = SessionManager.IsAdministrator;
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Media", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    private TransitAccountFeedItemMediaQueryOptions mQueryOptions = null;

    TransitAccountFeedItemMediaQueryOptions QueryOptions
    {
        get
        {
            if (mQueryOptions == null)
            {
                mQueryOptions = new TransitAccountFeedItemMediaQueryOptions();
                mQueryOptions.InterestingOnly = false;
                mQueryOptions.VisibleOnly = (IsEditing ? false : true);
            }

            return mQueryOptions;
        }
    }

    public void linkEdit_Click(object sender, EventArgs e)
    {
        IsEditing = !IsEditing;
        linkEdit.Text = IsEditing ? "&#187; Preview" : "&#187; Edit";
        GetData();
    }

    private void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountFeedItemMedia, TransitAccountFeedItemMediaQueryOptions>(
            QueryOptions, SessionManager.SyndicationService.GetAccountFeedItemMediasCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();

        int feedsCount = SessionManager.GetCount<TransitAccountFeed>(
            SessionManager.SyndicationService.GetAllAccountFeedsCount);

        labelCount.Text = string.Format("{0} media item{1} from <a href='AccountFeedsView.aspx'>{2} blog{3}</a>",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount == 1 ? string.Empty : "s",
            feedsCount, feedsCount == 1 ? string.Empty : "s");
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountFeedItemMedia, TransitAccountFeedItemMediaQueryOptions>(
            QueryOptions, serviceoptions, SessionManager.SyndicationService.GetAccountFeedItemMedias);
    }

    public void gridManage_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Toggle":
                TransitAccountFeedItemMedia img = SessionManager.SyndicationService.GetAccountFeedItemMediaById(
                    SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                img.Visible = !img.Visible;
                if (!img.Visible) img.Interesting = false;
                SessionManager.SyndicationService.CreateOrUpdateAccountFeedItemMedia(SessionManager.Ticket, img);
                LinkButton lb = (LinkButton)e.Item.FindControl("linkToggleVisible");
                lb.Text = img.Visible ? "&#187; Hide" : "&#187; Show";
                UpdatePanel up = (UpdatePanel)e.Item.FindControl("panelShowHide");
                up.Update();
                break;
        }
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }
}