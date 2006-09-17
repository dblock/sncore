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
using SnCore.Tools.Web;

public partial class AccountChangePassword : AuthenticatedPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAccountChangePassword);

            if (!string.IsNullOrEmpty(PasswordHash))
            {
                panelOldPassword.Visible = false;
            }

            if (SessionManager.Account.IsPasswordExpired)
            {
                ReportInfo("Your password has expired. Please change your password.");
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string PasswordHash
    {
        get
        {
            object hash = Request.QueryString["PasswordHash"];
            if (hash == null) return string.Empty;
            return hash.ToString();
        }
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? string.Empty : o.ToString());
        }
    }

    protected void changePassword_Click(object sender, EventArgs e)
    {
        try
        {
            if (inputNewPassword.Text != inputNewPassword2.Text)
            {
                throw new ArgumentException("Passwords don't match.");
            }

            if (!string.IsNullOrEmpty(PasswordHash))
            {
                AccountService.ChangePasswordMd5(
                    SessionManager.Ticket,
                    (RequestId > 0) ? RequestId : SessionManager.Account.Id,
                    PasswordHash, inputNewPassword.Text);
            }
            else
            {
                AccountService.ChangePassword(
                    SessionManager.Ticket,
                    (RequestId > 0) ? RequestId : SessionManager.Account.Id,
                    inputOldPassword.Text, inputNewPassword.Text);
            }

            ReportInfo("Password changed.");
            panelChangePassword.Visible = false;

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                Redirect(ReturnUrl);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
