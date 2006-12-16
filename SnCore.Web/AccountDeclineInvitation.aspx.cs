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
using SnCore.WebServices;
using SnCore.SiteMap;

[SiteMapDataAttribute("Decline Invitation")]
public partial class AccountDeclineInvitation : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(inputDecline);

            if (!IsPostBack)
            {
                TransitAccountInvitation invitation = SessionManager.AccountService.GetAccountInvitationById(
                    SessionManager.Ticket, RequestId);

                linkAccount.Text = Render(invitation.AccountName);
                linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", invitation.AccountId);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected void decline_Click(object sender, EventArgs e)
    {
        try
        {
            SessionManager.AccountService.DeclineInvitation(RequestId, Request.QueryString["code"]);
            panelDecline.Visible = false;
            ReportInfo("Invation declined. Click <a href='AccountCreate.aspx'>here</a> to join without an invitation.");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
