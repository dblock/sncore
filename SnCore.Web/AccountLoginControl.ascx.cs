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
using System.Web.Services.Protocols;
using SnCore.Tools.Web;
using SnCore.Services;
using System.Collections.Specialized;

public partial class AccountLoginControl : Control
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PageManager.SetDefaultButton(loginLogin, Controls);
            if (!IsPostBack)
            {
                linkAdministrator.OnClientClick =
                    string.Format("location.href='mailto:{0}';",
                        SystemService.GetConfigurationByNameWithDefault(
                            "SnCore.Admin.EmailAddress", "admin@localhost.com").Value);

                string openidmode = Request["openid.mode"];
                string openidtoken = SessionManager.OpenIdToken;
                if (!string.IsNullOrEmpty(openidmode) && !string.IsNullOrEmpty(openidtoken))
                {
                    string ticket = AccountService.LoginOpenId(openidtoken, Request.Params);
                    SessionManager.Login(ticket, SessionManager.RememberLogin);
                    Redirect(ReturnUrl);
                }
            }
        }
        catch (Exception ex)
        {
            SessionManager.Logout();
            ReportException(ex);
        }
    }

    protected void loginLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string ticket;
            if (!string.IsNullOrEmpty(loginEmailAddress.Text))
            {
                ticket = AccountService.Login(loginEmailAddress.Text, loginPassword.Text);
                SessionManager.Login(ticket, loginRememberMe.Checked);
                Redirect(ReturnUrl);
            }
            else if (!string.IsNullOrEmpty(loginOpenId.Text))
            {
                // url root needs to be a case-sensitive match for the openid server trust
                TransitOpenIdRedirect redirect = AccountService.GetOpenIdRedirect(loginOpenId.Text, Request.Url.ToString());
                SessionManager.OpenIdToken = redirect.Token;
                SessionManager.RememberLogin = loginRememberMe.Checked;
                Redirect(redirect.Url);
            }
            else
            {
                throw new ManagedAccount.AccessDeniedException();
            }
        }
        catch (Exception ex) 
        {
            SessionManager.Logout();
            ReportException(ex); 
        }
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "Default.aspx" : o.ToString());
        }
    }
}
