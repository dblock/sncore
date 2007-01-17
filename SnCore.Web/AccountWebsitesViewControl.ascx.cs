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
using Wilco.Web.UI.WebControls;
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountWebsitesViewControl : Control
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
        accountWebsites.OnGetDataSource += new EventHandler(accountWebsites_OnGetDataSource);
        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        accountWebsites.CurrentPageIndex = 0;
        accountWebsites.VirtualItemCount = SessionManager.GetCount<TransitAccountWebsite, int>(
            AccountId, SessionManager.AccountService.GetAccountWebsitesCount);
        accountWebsites_OnGetDataSource(sender, e);
        accountWebsites.DataBind();
        this.Visible = (accountWebsites.VirtualItemCount > 0);
    }

    void accountWebsites_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(accountWebsites.PageSize, accountWebsites.CurrentPageIndex);
        accountWebsites.DataSource = SessionManager.GetCollection<TransitAccountWebsite, int>(
            AccountId, options, SessionManager.AccountService.GetAccountWebsites);
        panelGrid.Update();
    }
}
