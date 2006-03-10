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
                accountFeeds_OnGetDataSource(sender, e);
                accountFeeds.DataBind();
                this.Visible = accountFeeds.Items.Count > 0;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void accountFeeds_OnGetDataSource(object sender, EventArgs e)
    {
        accountFeeds.DataSource = SyndicationService.GetAccountFeedsById(SessionManager.Ticket, AccountId);
    }
}
