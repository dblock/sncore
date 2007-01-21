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
using SnCore.Services;

public partial class AccountMenuControl : Control
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SessionManager.IsLoggedIn)
            {
                TransitAccountMessageFolder inbox = SessionManager.AccountService.GetAccountMessageSystemFolder(
                        SessionManager.Ticket, SessionManager.AccountId, "inbox");

                if (inbox != null)
                {
                    linkRequestsStar.Visible = (inbox.UnReadMessageCount > 0);
                    linkInbox.InnerText = string.Format("Inbox ({0})", inbox.UnReadMessageCount);
                }

                linkRequests.InnerText = string.Format("Requests ({0})",
                    SessionManager.SocialService.GetAccountFriendRequestsCount(
                        SessionManager.Ticket, SessionManager.AccountId));
            }
        }
    }
}
