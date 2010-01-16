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

public partial class EmailAccountInvitation : AuthenticatedPage
{
    private TransitAccountInvitation mAccountInvitation;

    public TransitAccountInvitation AccountInvitation
    {
        get
        {
            if (mAccountInvitation == null)
            {
                mAccountInvitation = SessionManager.AccountService.GetAccountInvitationById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccountInvitation;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("{0} invites you to join {1}",
            Render(AccountInvitation.AccountName),
            Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")));
        panelMessage.Visible = !string.IsNullOrEmpty(AccountInvitation.Message);
    }

    public string InviteToUrl
    {
        get
        {
            return AccountInvitation.AccountGroupId != 0
                ? string.Format("AccountGroupView.aspx?id={0}", AccountInvitation.AccountGroupId)
                : SessionManager.WebsiteUrl;
        }
    }

    public string InviteTo
    {
        get
        {
            string websiteName = SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore");
            return AccountInvitation.AccountGroupId != 0
                ? string.Format("{0}: {1}", websiteName, AccountInvitation.AccountGroupName)
                : websiteName;
        }
    }
}

