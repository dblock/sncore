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

public partial class AccountChangePassword : AuthenticatedPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(manageAccountChangePassword);
    }

    protected void changePassword_Click(object sender, EventArgs e)
    {
        try
        {
            if (inputNewPassword.Text != inputNewPassword2.Text)
            {
                throw new ArgumentException("Passwords don't match.");
            }

            AccountService.ChangePassword(SessionManager.Ticket, inputOldPassword.Text, inputNewPassword.Text);
            ReportInfo("Password changed.");
            panelChangePassword.Visible = false;
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
