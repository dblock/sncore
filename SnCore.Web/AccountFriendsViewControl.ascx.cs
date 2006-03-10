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

public partial class AccountFriendsViewControl : Control
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
            friendsList.OnGetDataSource += new EventHandler(friendsList_OnGetDataSource);

            if (!IsPostBack)
            {
                friendsList_OnGetDataSource(this, null);
                friendsList.DataBind();
                this.Visible = friendsList.Items.Count > 0;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void friendsList_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            friendsList.DataSource = SocialService.GetFriendsById(SessionManager.Ticket, AccountId);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
