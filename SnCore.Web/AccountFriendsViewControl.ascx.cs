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
        friendsList.CurrentPageIndex = 0;
        friendsList.VirtualItemCount = SocialService.GetFriendsCountById(AccountId);
        friendsList_OnGetDataSource(sender, e);
        friendsList.DataBind();
        this.Visible = (friendsList.VirtualItemCount > 0);
    }

    void friendsList_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = friendsList.CurrentPageIndex;
            options.PageSize = friendsList.PageSize;
            friendsList.DataSource = SocialService.GetFriendsById(SessionManager.Ticket, AccountId, options);
            panelGrid.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
