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
        try
        {
            accountFeeds.OnGetDataSource += new EventHandler(accountFeeds_OnGetDataSource);
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
        accountFeeds.CurrentPageIndex = 0;
        object[] args = { AccountId };
        accountFeeds.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            SyndicationService, "GetAccountFeedsCountById", args);
        accountFeeds_OnGetDataSource(sender, e);
        accountFeeds.DataBind();
        this.Visible = (accountFeeds.VirtualItemCount > 0);
    }

    void accountFeeds_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = accountFeeds.CurrentPageIndex;
        options.PageSize = accountFeeds.PageSize;
        object[] args = { SessionManager.Ticket, AccountId, options };
        accountFeeds.DataSource = SessionManager.GetCachedCollection<TransitAccountFeed>(
            SyndicationService, "GetAccountFeedsById", args);
        panelGrid.Update();
    }
}
