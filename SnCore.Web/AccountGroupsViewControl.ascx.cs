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
using Wilco.Web.UI;
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountGroupsViewControl : Control
{
    public int AccountId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountId", 0);
        }
        set
        {
            ViewState["AccountId"] = value;
        }
    }

    public bool PublicOnly
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "PublicOnly", true);
        }
        set
        {
            ViewState["PublicOnly"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        groupsList.OnGetDataSource += new EventHandler(groupsList_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        groupsList.CurrentPageIndex = 0;
        if (PublicOnly)
        {
            groupsList.VirtualItemCount = SessionManager.GetCount<TransitAccountGroupAccount, int>(
                AccountId, SessionManager.GroupService.GetPublicAccountGroupAccountsByAccountIdCount);
        }
        else
        {
            groupsList.VirtualItemCount = SessionManager.GetCount<TransitAccountGroupAccount, int>(
                AccountId, SessionManager.GroupService.GetAccountGroupAccountsByAccountIdCount);
        }
        groupsList_OnGetDataSource(sender, e);
        groupsList.DataBind();
        this.Visible = (groupsList.VirtualItemCount > 0);
    }

    void groupsList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(groupsList.PageSize, groupsList.CurrentPageIndex);
        if (PublicOnly)
        {
            groupsList.DataSource = SessionManager.GetCollection<TransitAccountGroupAccount, int>(
                AccountId, options, SessionManager.GroupService.GetPublicAccountGroupAccountsByAccountId);
        }
        else
        {
            groupsList.DataSource = SessionManager.GetCollection<TransitAccountGroupAccount, int>(
                AccountId, options, SessionManager.GroupService.GetAccountGroupAccountsByAccountId);
        }
        panelGrid.Update();
    }
}
