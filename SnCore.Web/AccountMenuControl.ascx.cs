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

public partial class AccountMenuControl : Control
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            panelSystem.Visible = SessionManager.IsLoggedIn && SessionManager.IsAdministrator;

            if (SessionManager.IsLoggedIn)
            {
                linkInbox.InnerText = string.Format("Inbox ({0})",
                    SessionManager.AccountService.GetAccountMessageSystemFolder(SessionManager.Ticket, "inbox").MessageCount);

                linkRequests.InnerText = string.Format("Requests ({0})",
                    SessionManager.SocialService.GetAccountFriendRequestsCountById(SessionManager.Account.Id));
            }

        }
    }
}
