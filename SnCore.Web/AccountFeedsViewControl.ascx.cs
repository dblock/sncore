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

public partial class AccountFeedsViewControl : Control
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
        accountFeeds.OnGetDataSource += new EventHandler(accountFeeds_OnGetDataSource);
        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        accountFeeds.CurrentPageIndex = 0;
        accountFeeds.VirtualItemCount = SessionManager.GetCount<TransitAccountFeed, int>(
            AccountId, SessionManager.SyndicationService.GetAccountFeedsCount);
        accountFeeds_OnGetDataSource(sender, e);
        accountFeeds.DataBind();
        this.Visible = (accountFeeds.VirtualItemCount > 0);
    }

    void accountFeeds_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = accountFeeds.CurrentPageIndex;
        options.PageSize = accountFeeds.PageSize;
        accountFeeds.DataSource = SessionManager.GetCollection<TransitAccountFeed, int>(
            AccountId, options, SessionManager.SyndicationService.GetAccountFeeds);
        panelGrid.Update();
    }
}
