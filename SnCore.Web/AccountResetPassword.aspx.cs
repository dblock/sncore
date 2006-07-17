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

public partial class AccountResetPassword : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(resetPassword);
    }

    protected void resetPassword_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(resetpasswordEmailAddress.Text))
            {
                throw new Exception("Please enter your e-mail address.");
            }

            if (!resetpasswordBirthday.HasDate)
            {
                throw new Exception("Please select a valid date.");
            }

            AccountService.ResetPassword(resetpasswordEmailAddress.Text, resetpasswordBirthday.SelectedDate);
            ReportInfo("A new password has been sent to '" + resetpasswordEmailAddress.Text + "'.");
            panelReset.Visible = false;
            panelResetUpdate.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
