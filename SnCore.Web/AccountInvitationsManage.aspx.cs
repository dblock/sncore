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
using System.Text;
using System.Text.RegularExpressions;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Generic;
using SnCore.Tools;
using System.Net.Mail;

public partial class AccountInvitationsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            if (!SessionManager.AccountService.HasVerifiedEmail(SessionManager.Ticket, SessionManager.AccountId))
            {
                ReportWarning("You don't have any verified e-mail addresses.\n" +
                    "You must add/confirm a valid e-mail address before inviting people.");

                panelInvite.Visible = false;
            }

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Invitations", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(invite);
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.AccountService.GetAccountInvitationsCount(
            SessionManager.Ticket, SessionManager.AccountId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.AccountService.GetAccountInvitations(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    public static Regex emailregex = new Regex(@".*@.*\..*", RegexOptions.Compiled);

    public void invite_Click(object sender, EventArgs e)
    {
        List<string> invitations = new List<string>();
        List<string> failures = new List<string>();
        ExceptionCollection exceptions = new ExceptionCollection();

        foreach (string email in inputEmailAddress.Text.Split(";,\n\r".ToCharArray()))
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email.Trim()))
                continue;

            try
            {
                TransitAccount existing = SessionManager.AccountService.FindByEmail(
                    SessionManager.Ticket, email);

                exceptions.Add(new Exception(string.Format(
                        "<a href='AccountView.aspx?id={0}'>{2} &lt;{3}&gt;</a> is a member! " +
                        "<a href='AccountFriendRequestEdit.aspx?pid={0}&ReturnUrl={1}'>&#187; Add to Friends</a>",
                        existing.Id, Request.Url.PathAndQuery, existing.Name, email)));

                continue;
            }
            catch
            {

            }

            try
            {
                MailAddress address = new MailAddress(email);
                TransitAccountInvitation invitation = new TransitAccountInvitation();
                invitation.Code = Guid.NewGuid().ToString();
                invitation.Email = email;
                invitation.Message = inputMessage.Text;
                SessionManager.AccountService.CreateOrUpdateAccountInvitation(SessionManager.Ticket, invitation);
                invitations.Add(email);
            }
            catch (Exception ex)
            {
                failures.Add(email);
                exceptions.Add(new Exception(string.Format("Error inviting {0}: {1}", email, ex.Message), ex));
            }
        }

        if (invitations.Count > 0)
        {
            noticeManage.Info = string.Format("{0} invitation{1} sent",
                invitations.Count, invitations.Count == 1 ? string.Empty : "s");
        }

        GetData(sender, e);
        inputEmailAddress.Text = string.Join("\n", failures.ToArray());
        exceptions.Throw();
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                switch (e.CommandName)
                {
                    case "Delete":
                        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                        SessionManager.AccountService.DeleteAccountInvitation(SessionManager.Ticket, id);
                        ReportInfo("Invitation deleted.");
                        gridManage.CurrentPageIndex = 0;
                        gridManage_OnGetDataSource(source, e);
                        gridManage.DataBind();
                        break;
                }
                break;
        }
    }

}
