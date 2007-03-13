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
using SnCore.SiteMap;

public partial class AccountGroupPlacesView : Page
{
    private bool mAccountGroupAccountRetreived = false;
    private TransitAccountGroupAccount mAccountGroupAccount = null;

    private TransitAccountGroupAccount AccountGroupAccount
    {
        get
        {
            if (! mAccountGroupAccountRetreived)
            {
                mAccountGroupAccountRetreived = true;

                if (SessionManager.IsLoggedIn)
                {
                    mAccountGroupAccount = SessionManager.GroupService.GetAccountGroupAccountByAccountGroupId(
                        SessionManager.Ticket, SessionManager.AccountId, RequestId);
                }
            }
            return mAccountGroupAccount;
        }
    }

    public bool IsGroupAdministrator()
    {
        if (SessionManager.IsAdministrator)
            return true;

        if (AccountGroupAccount == null)
            return false;

        return AccountGroupAccount.IsAdministrator;
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            TransitAccountGroup ta = SessionManager.GroupService.GetAccountGroupById(
                SessionManager.Ticket, RequestId);

            labelName.Text = string.Format("{0}'s Places", Render(ta.Name));
            linkAccountGroup.Text = string.Format("&#187; Back to {0}", Render(ta.Name));
            linkAccountGroup.NavigateUrl = string.Format("AccountGroupView.aspx?id={0}", ta.Id);
            linkRelRss.NavigateUrl = string.Format("AccountGroupPlacesRss.aspx?id={0}", ta.Id);

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(ta.Name, Request, string.Format("AccountGroupView.aspx?id={0}", ta.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountGroupPlace, int>(
            RequestId, SessionManager.GroupService.GetAccountGroupPlacesCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountGroupPlace, int>(
            RequestId, options, SessionManager.GroupService.GetAccountGroupPlaces);
    }

    public void gridManage_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.Delete<TransitAccountGroupPlace>(id, SessionManager.GroupService.DeleteAccountGroupPlace);
                    ReportInfo("Member deleted.");
                    GetData(sender, e);
                    break;
                }
        }
    }
}
