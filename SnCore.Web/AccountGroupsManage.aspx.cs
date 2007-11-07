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
using SnCore.WebServices;
using SnCore.SiteMap;
using SnCore.Services;

public partial class AccountGroupsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GroupService.GetAccountGroupAccountsByAccountIdCount(
            SessionManager.Ticket, SessionManager.AccountId);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GroupService.GetAccountGroupAccountsByAccountId(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.Delete<TransitAccountGroup>(id, SessionManager.GroupService.DeleteAccountGroup);
                    ReportInfo("Group deleted.");
                    GetData();
                    break;
                }
            case "Leave":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.Delete<TransitAccountGroupAccount>(id, SessionManager.GroupService.DeleteAccountGroupAccount);
                    ReportInfo("Group deleted.");
                    GetData();
                    break;
                }
        }
    }
}
