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
using System.Collections.Generic;
using System.Collections.Specialized;
using SnCore.WebControls;

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
                       SessionManager.GetCachedConfiguration(
                            "SnCore.Admin.EmailAddress", "admin@localhost.com"));

                FacebookPageManager facebook = new FacebookPageManager(SessionManager);

                if (string.IsNullOrEmpty(facebook.FacebookAPIKey))
                {
                    panelFacebookLogin.Visible = false;
                    facebookLoginDisabled.Visible = true;
                }

                string openidmode = Request["openid.mode"];
                string openidtoken = SessionManager.OpenIdToken;
                if (!string.IsNullOrEmpty(openidmode) && !string.IsNullOrEmpty(openidtoken))
                {
                    NameValueCollectionSerializer serializer = new NameValueCollectionSerializer(Request.Params);
                    TransitOpenIdLogin t_login = SessionManager.AccountService.TryLoginOpenId(openidtoken, serializer.Names, serializer.Values);
                    if (!string.IsNullOrEmpty(t_login.Ticket))
                    {
                        SessionManager.Login(t_login.Ticket, SessionManager.RememberLogin);
                        Redirect(ReturnUrl);
                    }
                    else
                    {
                        Redirect(string.Format("AccountCreateOpenId.aspx?ReturnUrl={0}&ConsumerUrl={1}",
                            Renderer.UrlEncode(ReturnUrl), Renderer.UrlEncode(t_login.ConsumerUrl)));
                    }
                }

                string facebookConnect = Request["connect"];
                if (!string.IsNullOrEmpty(facebookConnect))
                {
                    SortedList<string, string> facebookCookies = facebook.GetFacebookCookies(HttpContext.Current.Request.Cookies);
                    List<String> keys = new List<String>(facebookCookies.Keys);
                    List<String> values = new List<String>(facebookCookies.Values);
                    TransitFacebookLogin t_login = SessionManager.AccountService.TryLoginFacebook(
                        HttpContext.Current.Request.Cookies[facebook.FacebookAPIKey].Value, keys.ToArray(), values.ToArray());

                    if (string.IsNullOrEmpty(t_login.Ticket))
                    {
                        Redirect("AccountCreateFacebook.aspx?connect=1");
                        return;
                    }

                    SessionManager.Login(t_login.Ticket, SessionManager.RememberLogin);
                    Redirect(ReturnUrl);
                }

                if (SessionManager.IsLoggedIn)
                {
                    ReportInfo("You're already logged-in. Click <a href='AccountManage.aspx'>here</a> to continue.", false);
                    loginLogin.Enabled = false;
                }
            }
        }
        catch
        {
            SessionManager.Logout();
            throw;
        }
    }

    protected void loginLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string ticket;
            if (!string.IsNullOrEmpty(loginEmailAddress.Text))
            {
                ticket = SessionManager.AccountService.Login(loginEmailAddress.Text, loginPassword.Text.Trim());
                SessionManager.Login(ticket, loginRememberMe.Checked);

                TransitAccount ta = SessionManager.AccountService.GetAccount(ticket, true);
                if (ta != null && ta.IsPasswordExpired)
                {
                    Redirect(string.Format("AccountChangePassword.aspx?ReturnUrl={0}&PasswordHash={1}",
                        Renderer.UrlEncode(ReturnUrl), Renderer.UrlEncode(ManagedAccount.GetPasswordHash(loginPassword.Text))));
                }
                else
                {
                    Redirect(ReturnUrl);
                }
            }
            else if (!string.IsNullOrEmpty(loginOpenId.Text))
            {
                // url root needs to be a case-sensitive match for the openid server trust
                TransitOpenIdRedirect redirect = SessionManager.AccountService.GetOpenIdRedirect(loginOpenId.Text, Request.Url.ToString());
                SessionManager.OpenIdToken = redirect.Token;
                SessionManager.RememberLogin = loginRememberMe.Checked;
                Redirect(redirect.Url);
            }
            else
            {
                throw new ManagedAccount.AccessDeniedException();
            }
        }
        catch
        {
            SessionManager.Logout();
            throw;
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

    public string FacebookLoginUri
    {
        get
        {
            FacebookPageManager facebook = new FacebookPageManager(SessionManager);
            return facebook.GetLoginUrl(Request.Url.ToString());
        }
    }
}
