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
        try
        {
            accountWebsites.OnGetDataSource += new EventHandler(accountWebsites_OnGetDataSource);
            if (!IsPostBack)
            {
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        accountWebsites.CurrentPageIndex = 0;
        object[] args = { AccountId };
        accountWebsites.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            SessionManager.AccountService, "GetAccountWebsitesCountById", args);
        accountWebsites_OnGetDataSource(sender, e);
        accountWebsites.DataBind();
        this.Visible = (accountWebsites.VirtualItemCount > 0);
    }

    void accountWebsites_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(accountWebsites.PageSize, accountWebsites.CurrentPageIndex);
        object[] args = { AccountId, options };
        accountWebsites.DataSource = SessionManager.GetCachedCollection<TransitAccountWebsite>(
            SessionManager.AccountService, "GetAccountWebsitesById", args);
        panelGrid.Update();
    }
}
