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
        groupsList.VirtualItemCount = SessionManager.GetCount<TransitAccountGroupAccount, int>(
            AccountId, SessionManager.GroupService.GetPublicAccountGroupAccountsByAccountIdCount);
        groupsList_OnGetDataSource(sender, e);
        groupsList.DataBind();
        this.Visible = (groupsList.VirtualItemCount > 0);
    }

    void groupsList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(groupsList.PageSize, groupsList.CurrentPageIndex);
        groupsList.DataSource = SessionManager.GetCollection<TransitAccountGroupAccount, int>(
            AccountId, options, SessionManager.GroupService.GetPublicAccountGroupAccountsByAccountId);
        panelGrid.Update();
    }
}
