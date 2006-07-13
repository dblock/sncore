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

public partial class AccountInvitationsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            SetDefaultButton(invite);
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
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = AccountService.GetAccountInvitationsCount(SessionManager.Ticket);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = AccountService.GetAccountInvitations(SessionManager.Ticket, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public static Regex emailregex = new Regex(@".*@.*\..*", RegexOptions.Compiled);

    public void invite_Click(object sender, EventArgs e)
    {
        StringBuilder error = new StringBuilder();
        StringBuilder success = new StringBuilder();

        noticeManage.HtmlEncode = true;
        noticeManage.Error = string.Empty;

        try
        {
            foreach (string email in inputEmailAddress.Text.Split(";,\n\r".ToCharArray()))
            {
                if (email.Length == 0)
                    continue;

                try
                {
                    TransitAccount existing = AccountService.FindByEmail(email);

                    error.AppendFormat(
                            "An account with the e-mail address {3} already exists." +
                            "<ul><li><a href='AccountFriendRequestEdit.aspx?pid={0}&ReturnUrl={1}'>Send {2} &lt;{3}&gt; a friend request.</a>" +
                            "<li><a href='AccountView.aspx?id={0}'>View {2} &lt;{3}&gt;.</a></ul><br>\n",
                            existing.Id, Request.Url.PathAndQuery, existing.Name, email);

                    continue;
                }
                catch
                {
                }

                if (! emailregex.Match(email).Success)
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
                    AccountService.AddAccountInvitation(SessionManager.Ticket, invitation);
                    success.AppendFormat("Invitation sent to {0}.<br>", email);
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
                catch (Exception ex)
                {
                    error.Append(ex.Message);
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
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
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
                            AccountService.DeleteAccountInvitation(SessionManager.Ticket, id);
                            ReportInfo("Invitation deleted.");
                            gridManage.CurrentPageIndex = 0;
                            gridManage_OnGetDataSource(source, e);
                            gridManage.DataBind();
                            break;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
