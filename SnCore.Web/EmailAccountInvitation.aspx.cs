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
            try
            {
                if (mAccountInvitation == null)
                {
                    mAccountInvitation = SessionManager.AccountService.GetAccountInvitationById(
                        SessionManager.Ticket, RequestId);
                }
            }
            catch (Exception ex)
            {
                ReportException(ex);
            }

            return mAccountInvitation;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Title = string.Format("{0} invites you to join {1}", 
                Render(AccountInvitation.AccountName), 
                Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")));
            panelMessage.Visible = ! string.IsNullOrEmpty(AccountInvitation.Message);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

