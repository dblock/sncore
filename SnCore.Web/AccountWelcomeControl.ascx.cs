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
using System.Collections.Generic;
using SnCore.Tools.Web;

public partial class AccountWelcomeControl : Control
{
    public void Page_Load()
    {
        if (!IsPostBack)
        {
            if (SessionManager.IsLoggedIn)
            {
                panelLoggedIn.Visible = true;
                panelJoin.Visible = false;
                loggedInAccountName.Text = Renderer.Render(SessionManager.Account.Name);

                TransitAccountMessageFolder inbox = SessionManager.GetInstance<TransitAccountMessageFolder, int, string>(
                    SessionManager.AccountId, "inbox", SessionManager.AccountService.GetAccountMessageSystemFolder);

                if (inbox != null)
                {
                    linkRequestsStar.Visible = (inbox.UnReadMessageCount > 0);
                    linkInbox.InnerText = string.Format("{0} ({1})",
                        HttpContext.GetGlobalResourceObject("Links", "Inbox"),
                        inbox.UnReadMessageCount);
                }

                linkRequests.InnerText = string.Format("{0} ({1})",
                    HttpContext.GetGlobalResourceObject("Links", "Requests"),
                    SessionManager.GetCount<TransitAccountFriendRequest, int>(
                        SessionManager.AccountId, SessionManager.SocialService.GetAccountFriendRequestsCount));

                linkInvitations.InnerText = string.Format("{0} ({1})",
                    HttpContext.GetGlobalResourceObject("Links", "Invitations"),
                    SessionManager.GetCount<TransitAccountGroupAccountInvitation, int>(
                        SessionManager.AccountId, SessionManager.GroupService.GetAccountGroupAccountInvitationsByAccountIdCount));
            }
            else
            {
                panelLoggedIn.Visible = false;
                panelJoin.Visible = true;
                int accountsCount = SessionManager.GetCount<TransitAccount>(SessionManager.AccountService.GetAccountsCount);
                if (accountsCount > 100) accountsCount = accountsCount / 10 * 10;
                if (accountsCount > 1000) accountsCount = accountsCount / 100 * 100;
                panelJoinLinkAccounts.Text = accountsCount.ToString();
                panelJoinWebsiteName.Text = Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"));
            }
        }
    }
}
