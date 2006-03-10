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

public partial class AccountEmailVerify : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(inputVerify);
        if (!IsPostBack)
        {
            inputCode.Text = Request.QueryString["code"];
        }
    }

    public void EmailVerify_Click(object sender, EventArgs e)
    {
        try
        {
            string emailaddress = AccountService.VerifyEmail(
                inputPassword.Text, 
                RequestId, 
                inputCode.Text);

            if (!SessionManager.IsLoggedIn)
            {
                string ticket = AccountService.Login(emailaddress, inputPassword.Text);
                SessionManager.Login(ticket, true);
            }

            panelVerify.Visible = false;
            ReportInfo("Thank you. Your e-mail address has been verified." +
                "<br>Click <a href='Default.aspx'>here</a> to continue.", false);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
