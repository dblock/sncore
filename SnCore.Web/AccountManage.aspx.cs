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
using System.Collections.Generic;
using System.Web.Caching;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;
using System.Text;

[SiteMapDataAttribute("Me Me")]
public partial class AccountManage : AuthenticatedPage
{
    private bool NotifyNoVerifiedEmail(object sender, EventArgs e)
    {
        if (!SessionManager.HasVerified())
        {
            noticeVerifiedEmail.HtmlEncode = false;
            noticeVerifiedEmail.Warning = "You don't have a verified e-mail addresses and/or profile photos. " +
                "To help prevent spam, you will only be able to post once you have verified your e-mail and <a href='AccountPicturesManage.aspx'>uploaded a profile photo</a>. " +
                "If you haven't received a confirmation e-mail, please " +
                "<a href='AccountEmailsManage.aspx'>double-check your address</a>. ";                
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool NotifyEmailBounced(object sender, EventArgs e)
    {
        IList<TransitAccountEmail> emails = SessionManager.GetCollection<TransitAccountEmail, int>(
            SessionManager.AccountId, null, SessionManager.AccountService.GetAccountEmails);

        foreach (TransitAccountEmail email in emails)
        {
            if (email.Failed)
            {
                noticeVerifiedEmail.HtmlEncode = false;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("We tried to send you an e-mail to \"{0}\", which bounced. ", Renderer.Render(email.Address));
                if (! string.IsNullOrEmpty(email.LastError)) sb.AppendFormat("The error was \"{0}\". ", Renderer.Render(email.LastError));
                sb.Append("Please <a href='AccountEmailsManage.aspx'>double-check your address</a>.");
                noticeVerifiedEmail.Warning = sb.ToString();
                return true;
            }
        }

        return false;
    }

    private bool NotifyInvitationBounced(object sender, EventArgs e)
    {
        IList<TransitAccountInvitation> invitations = SessionManager.GetCollection<TransitAccountInvitation, int>(
            SessionManager.AccountId, null, SessionManager.AccountService.GetAccountInvitationsByAccountId);

        foreach (TransitAccountInvitation invitation in invitations)
        {
            if (invitation.Failed)
            {
                noticeVerifiedEmail.HtmlEncode = false;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("We tried to invite your friend \"{0}\", but the e-mail bounced. ", Renderer.Render(invitation.Email));
                if (!string.IsNullOrEmpty(invitation.LastError)) sb.AppendFormat("The error was \"{0}\". ", Renderer.Render(invitation.LastError));
                sb.Append("Please <a href='AccountInvitationsManage.aspx'>double-check this address</a>.");
                noticeVerifiedEmail.Warning = sb.ToString();
                return true;
            }
        }

        return false;
    }

    private void NotifyBounces(object sender, EventArgs e)
    {
        if (NotifyEmailBounced(sender, e))
            return;

        if (NotifyNoVerifiedEmail(sender, e))
            return;

        if (NotifyInvitationBounced(sender, e))
            return;
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            NotifyBounces(sender, e);

            accountName.Text = string.Format("Hello, {0}!", Renderer.Render(SessionManager.Account.Name));
            accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", SessionManager.Account.PictureId);

            groupsView.AccountId = SessionManager.AccountId;
            friendsView.AccountId = SessionManager.AccountId;
            broadcastView.AccountId = SessionManager.AccountId;

            TransitAccountNumbers numbers = SessionManager.GetInstance<TransitAccountNumbers, int>(
                SessionManager.AccountId, SessionManager.AccountService.GetAccountNumbersByAccountId);

            accountFirstDegree.Visible = (numbers.FirstDegreeCount > 0);
            accountFirstDegree.Text = string.Format("{0} friend{1} in your personal network",
                numbers.FirstDegreeCount,
                numbers.FirstDegreeCount != 1 ? "s" : string.Empty);

            accountSecondDegree.Visible = (numbers.SecondDegreeCount > 0);
            accountSecondDegree.Text = string.Format("{0} friend{1} in your extended network",
                numbers.SecondDegreeCount,
                numbers.SecondDegreeCount != 1 ? "s" : string.Empty);

            accountAllDegrees.Text = string.Format("{0} {1} to make new friends with",
                numbers.NewCount > 0 ? numbers.NewCount.ToString() : "no",
                numbers.NewCount != 1 ? "people" : "person");

            accountDiscussionThreads.Visible = (numbers.PostsCount > 0);
            accountDiscussionThreads.Text = string.Format("{0} discussion post{1}",
                numbers.PostsCount, numbers.PostsCount != 1 ? "s" : string.Empty);
        }
    }
}
