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
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountFriendsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            friendsList.OnGetDataSource += new EventHandler(friendsList_OnGetDataSource);

            if (!IsPostBack)
            {
                friendsList_OnGetDataSource(this, null);
                friendsList.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void friendsList_OnGetDataSource(object sender, EventArgs e)
    {
        friendsList.DataSource = SocialService.GetFriends(SessionManager.Ticket);
    }

    public void friendsList_Command(object sender, DataListCommandEventArgs e)
    {
        try
        {            
            switch (e.CommandName)
            {
                case "Delete":
                    SocialService.DeleteAccountFriend(SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                    friendsList_OnGetDataSource(sender, e);
                    friendsList.DataBind();
                    ReportInfo("Friend deleted.");
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
