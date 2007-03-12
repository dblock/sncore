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
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountGroupAccountsViewControl : Control
{
    public int AccountGroupId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountGroupId", 0);
        }
        set
        {
            ViewState["AccountGroupId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        friendsList.OnGetDataSource += new EventHandler(friendsList_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            linkAll.Text = string.Format("&#187; {0} member{1}",
                friendsList.VirtualItemCount, friendsList.VirtualItemCount == 1 ? string.Empty : "s");
            linkAll.NavigateUrl = string.Format("AccountGroupAccountsView.aspx?id={0}", AccountGroupId);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        friendsList.CurrentPageIndex = 0;
        friendsList.VirtualItemCount = SessionManager.GetCount<TransitAccountGroupAccount, int>(
            AccountGroupId, SessionManager.GroupService.GetAccountGroupAccountsCount);
        friendsList_OnGetDataSource(sender, e);
        friendsList.DataBind();
        this.Visible = (friendsList.VirtualItemCount > 0);
    }

    void friendsList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = friendsList.CurrentPageIndex;
        options.PageSize = friendsList.PageSize;
        friendsList.DataSource = SessionManager.GetCollection<TransitAccountGroupAccount, int>(
            AccountGroupId, options, SessionManager.GroupService.GetAccountGroupAccounts);
        panelGrid.Update();
    }
}
