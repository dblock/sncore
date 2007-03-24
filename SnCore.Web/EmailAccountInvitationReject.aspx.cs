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

public partial class EmailAccountInvitationReject : AuthenticatedPage
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
        Title = string.Format("Your friend declined your invitation to join {0}",
            Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")));
    }
}

