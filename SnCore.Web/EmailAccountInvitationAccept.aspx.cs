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
using SnCore.Tools.Web;

public partial class EmailAccountInvitationAccept : AuthenticatedPage
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

    private TransitAccount mAccount;

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = SessionManager.AccountService.GetAccountById(
                    SessionManager.Ticket, GetId("aid"));
            }
            return mAccount;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("Your friend {0} accepted your invitation to join {1}",
            Renderer.Render(Account.Name),
            Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")));
    }
}

