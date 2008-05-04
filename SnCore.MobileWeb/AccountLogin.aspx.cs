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
using SnCore.SiteMap;
using AccountService;

[SiteMapDataAttribute("Login")]
public partial class AccountLogin : Page
{
    public bool AuthenticatedPage
    {
        get
        {
            bool result = false;
            object ap = Request.Params["AuthenticatedPage"];
            if (ap == null) return false;
            bool.TryParse(ap.ToString(), out result);
            return result;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string openidmode = Request["openid.mode"];
                string openidtoken = SessionManager.OpenIdToken;
                if (!string.IsNullOrEmpty(openidmode) && !string.IsNullOrEmpty(openidtoken))
                {
                    NameValueCollectionSerializer serializer = new NameValueCollectionSerializer(Request.Params);
                    string ticket = SessionManager.AccountService.LoginOpenId(openidtoken, serializer.Names, serializer.Values);
                    SessionManager.Login(ticket, SessionManager.RememberLogin);
                    Redirect(ReturnUrl);
                    return;
                }

                if (AuthenticatedPage)
                {
                    ReportInfo("This is a members only page. You must first login. There're many perks of being a member " +
                        "and it's free. Click <a href=\"AccountCreate.aspx\">here</a> to join!");
                }
            }
            catch
            {
                SessionManager.Logout();
                throw;
            }
        }
    }

    public void loginLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string ticket;
            if (!string.IsNullOrEmpty(loginEmailAddress.Text))
            {
                ticket = SessionManager.AccountService.Login(loginEmailAddress.Text.Trim(), loginPassword.Text.Trim());
                SessionManager.Login(ticket, loginRememberMe.Checked);

                TransitAccount ta = SessionManager.AccountService.GetAccount(ticket, true);
                if (ta != null && ta.IsPasswordExpired)
                {
                    throw new Exception("Password expired. You cannot reset it on the mobile site.");
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
                throw new Exception("Access Denied"); // todo: typed access denied exception
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
}
