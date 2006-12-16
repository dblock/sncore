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

public partial class AccountInvitationsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            if (!SessionManager.AccountService.HasVerifiedEmail(SessionManager.Ticket))
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
        gridManage.VirtualItemCount = SessionManager.AccountService.GetAccountInvitationsCount(SessionManager.Ticket);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.AccountService.GetAccountInvitations(SessionManager.Ticket, options);
    }

    public static Regex emailregex = new Regex(@".*@.*\..*", RegexOptions.Compiled);

    public void invite_Click(object sender, EventArgs e)
    {
        StringBuilder error = new StringBuilder();
        StringBuilder success = new StringBuilder();

        noticeManage.HtmlEncode = true;
        noticeManage.Error = string.Empty;

        foreach (string email in inputEmailAddress.Text.Split(";,\n\r".ToCharArray()))
        {
            if (email.Length == 0)
                continue;

            try
            {
                TransitAccount existing = SessionManager.AccountService.FindByEmail(email);

                error.AppendFormat(
                        "<a href='AccountView.aspx?id={0}'>{2} &lt;{3}&gt;</a> is a member! " +
                        "<a href='AccountFriendRequestEdit.aspx?pid={0}&ReturnUrl={1}'>&#187; Add to Friends</a>\n",
                        existing.Id, Request.Url.PathAndQuery, existing.Name, email);

                continue;
            }
            catch
            {
            }

            if (!emailregex.Match(email).Success)
            {
                error.AppendFormat("Invalid e-mail address \"{0}\".<br>", email);
                continue;
            }

            try
            {
                TransitAccountInvitation invitation = new TransitAccountInvitation();
                invitation.Code = Guid.NewGuid().ToString();
                invitation.Email = email;
                invitation.Message = inputMessage.Text;
                SessionManager.AccountService.AddAccountInvitation(SessionManager.Ticket, invitation);
                success.AppendFormat("Invitation sent to {0}.<br>", email);
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();
            }
            catch (Exception ex)
            {
                error.AppendFormat("Error inviting {0}: {1}<br>", email, ex.Message);
            }
        }

        if (error.Length > 0)
        {
            noticeManage.HtmlEncode = false;
            noticeManage.Error = error.ToString();
        }
        else
        {
            inputEmailAddress.Text = string.Empty;
            inputMessage.Text = string.Empty;
        }

        if (success.Length > 0)
        {
            ReportInfo(success.ToString());
        }
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
