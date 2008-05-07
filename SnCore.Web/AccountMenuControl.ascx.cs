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

                linkAccountPlaceRequests.InnerText = string.Format("{0} ({1})",
                    HttpContext.GetGlobalResourceObject("Links", "Requests"),
                    SessionManager.GetCount<TransitAccountPlaceRequest, int>(
                        SessionManager.AccountId, SessionManager.PlaceService.GetAccountPlaceRequestsByAccountIdCount));
            }
        }
    }

    public void linkBroadcast_Click(object sender, EventArgs e)
    {
        // create a private audit entry for the broadcast draft
        TransitAccountAuditEntry audit_entry = new TransitAccountAuditEntry();
        audit_entry.AccountId = SessionManager.AccountId;
        audit_entry.Description = string.Format("Broadcast Draft: {0}", Guid.NewGuid());
        audit_entry.IsBroadcast = true;
        audit_entry.IsPrivate = true;
        audit_entry.IsSystem = false;
        audit_entry.Url = "AccountManage.aspx";
        audit_entry.Id = SessionManager.SocialService.CreateOrUpdateAccountAuditEntry(
            SessionManager.Ticket, audit_entry);
        // create a discussion attached to audit entry draft
        int discussion_id = SessionManager.DiscussionService.GetOrCreateDiscussionId(
            SessionManager.Ticket, "AccountAuditEntry", audit_entry.Id);
        Redirect(string.Format("DiscussionPost.aspx?did={0}&ReturnUrl=AccountManage.aspx", discussion_id));
    }
}
