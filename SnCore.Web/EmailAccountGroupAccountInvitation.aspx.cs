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

public partial class EmailAccountGroupAccountInvitation : AuthenticatedPage
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
        Title = string.Format("{0} invites you to join \"{1}\"", 
            Renderer.Render(AccountGroupAccountInvitation.RequesterName), 
            Renderer.Render(AccountGroupAccountInvitation.AccountGroupName));
    }
}

