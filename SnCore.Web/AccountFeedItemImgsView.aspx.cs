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
using System.Collections.Specialized;
using System.Text;

public partial class AccountFeedItemImgsView : AccountPersonPage
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
        ((SnCoreMasterPage)Master).ScriptManager.Navigate += new EventHandler<HistoryEventArgs>(History_Navigate);
        if (!IsPostBack)
        {
            linkEdit.Visible = SessionManager.IsAdministrator;
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Food Porn", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void History_Navigate(object sender, HistoryEventArgs e)
    {
        if (e.State.HasKeys())
        {
            gridManage.CurrentPageIndex = int.Parse(e.State["page"]);
        }
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    private TransitAccountFeedItemImgQueryOptions mQueryOptions = null;

    TransitAccountFeedItemImgQueryOptions QueryOptions
    {
        get
        {
            if (mQueryOptions == null)
            {
                mQueryOptions = new TransitAccountFeedItemImgQueryOptions();
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
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountFeedItemImg, TransitAccountFeedItemImgQueryOptions>(
            QueryOptions, SessionManager.SyndicationService.GetAccountFeedItemImgsCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();

        TransitAccountFeedQueryOptions options = new TransitAccountFeedQueryOptions();
        options.PublishedOnly = false;
        options.PicturesOnly = false;
        int feedsCount = SessionManager.GetCount<TransitAccountFeed, TransitAccountFeedQueryOptions>(
            options, SessionManager.SyndicationService.GetAccountFeedsCount);

        labelCount.Text = string.Format("{0} picture{1} from <a href='AccountFeedsView.aspx'>{2} blog{3}</a>",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount == 1 ? string.Empty : "s",
            feedsCount, feedsCount == 1 ? string.Empty : "s");
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountFeedItemImg, TransitAccountFeedItemImgQueryOptions>(
            QueryOptions, serviceoptions, SessionManager.SyndicationService.GetAccountFeedItemImgs);

        if (((SnCoreMasterPage)Master).ScriptManager.IsInAsyncPostBack &&
            !((SnCoreMasterPage)Master).ScriptManager.IsNavigating)
        {
            NameValueCollection history = new NameValueCollection();
            history.Add("page", gridManage.CurrentPageIndex.ToString());
            ((SnCoreMasterPage)Master).ScriptManager.AddHistoryPoint(history, Page.Title);
        }
    }

    public void gridManage_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Toggle":
                TransitAccountFeedItemImg img = SessionManager.SyndicationService.GetAccountFeedItemImgById(
                    SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                img.Visible = !img.Visible;
                if (!img.Visible) img.Interesting = false;
                SessionManager.CreateOrUpdate<TransitAccountFeedItemImg>(
                    img, SessionManager.SyndicationService.CreateOrUpdateAccountFeedItemImg);
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
