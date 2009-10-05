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
using nStuff.UpdateControls;
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
        ((SnCoreMasterPage)Master).History.Navigate += new HistoryEventHandler(History_Navigate);
        if (!IsPostBack)
        {
            linkEdit.Visible = SessionManager.IsAdministrator;
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void History_Navigate(object sender, nStuff.UpdateControls.HistoryEventArgs e)
    {
        string s = Encoding.Default.GetString(Convert.FromBase64String(e.EntryName));
        if (!string.IsNullOrEmpty(s))
        {
            NameValueCollection args = Renderer.ParseQueryString(s);
            gridManage.CurrentPageIndex = int.Parse(args["page"]);
        }
        else
        {
            gridManage.CurrentPageIndex = 0;
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

        string args = string.Format("page={0}", gridManage.CurrentPageIndex);
        if (!(e is nStuff.UpdateControls.HistoryEventArgs))
        {
            ((SnCoreMasterPage)Master).History.AddEntry(Convert.ToBase64String(Encoding.Default.GetBytes(args)));
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
