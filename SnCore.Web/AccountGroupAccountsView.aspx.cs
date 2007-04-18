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

public partial class AccountGroupAccountsView : AccountPersonPage
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

            Title = labelName.Text = string.Format("{0}'s Members", Render(ta.Name));
            linkAccountGroup.Text = string.Format("&#187; Back to {0}", Render(ta.Name));
            linkAccountGroup.NavigateUrl = string.Format("AccountGroupView.aspx?id={0}", ta.Id);
            linkRelRss.NavigateUrl = string.Format("AccountGroupAccountsRss.aspx?id={0}", ta.Id);

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(ta.Name, Request, string.Format("AccountGroupView.aspx?id={0}", ta.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Members", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountGroupAccount, int>(
            RequestId, SessionManager.GroupService.GetAccountGroupAccountsCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountGroupAccount, int>(
            RequestId, options, SessionManager.GroupService.GetAccountGroupAccounts);
    }

    public void gridManage_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.Delete<TransitAccountGroupAccount>(id, SessionManager.GroupService.DeleteAccountGroupAccount);
                    ReportInfo("Member deleted.");
                    GetData(sender, e);
                    break;
                }
            case "Promote":
                {
                    TransitAccountGroupAccount t_account = SessionManager.GroupService.GetAccountGroupAccountById(
                        SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                    t_account.IsAdministrator = true;
                    SessionManager.CreateOrUpdate<TransitAccountGroupAccount>(
                        t_account, SessionManager.GroupService.CreateOrUpdateAccountGroupAccount);
                    ReportInfo("Member promoted.");
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
                }
            case "Demote":
                {
                    TransitAccountGroupAccount t_account = SessionManager.GroupService.GetAccountGroupAccountById(
                        SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                    t_account.IsAdministrator = false;
                    SessionManager.CreateOrUpdate<TransitAccountGroupAccount>(
                        t_account, SessionManager.GroupService.CreateOrUpdateAccountGroupAccount);
                    ReportInfo("Member demoted.");
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
                }
        }
    }

}
