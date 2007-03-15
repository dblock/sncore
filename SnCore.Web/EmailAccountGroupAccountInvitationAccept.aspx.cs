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

public partial class EmailAccountGroupAccountInvitationAccept : AuthenticatedPage
{
    private TransitAccountGroupAccountInvitation mAccountGroupAccountInvitation;

    public TransitAccountGroupAccountInvitation AccountGroupAccountInvitation
    {
        get
        {
            if (mAccountGroupAccountInvitation == null)
            {
                mAccountGroupAccountInvitation = SessionManager.GroupService.GetAccountGroupAccountInvitationById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccountGroupAccountInvitation;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("Your friend {0} accepted your invitation to join {1}",
            Renderer.Render(AccountGroupAccountInvitation.AccountName),
            Renderer.Render(AccountGroupAccountInvitation.AccountGroupName));

        panelMessage.Visible = ! string.IsNullOrEmpty(Request.QueryString["message"]);
    }
}

