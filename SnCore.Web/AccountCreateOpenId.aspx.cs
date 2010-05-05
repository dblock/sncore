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
using SnCore.SiteMap;
using SnCore.Tools;

[SiteMapDataAttribute("Join with Open-Id")]
public partial class AccountCreateOpenId : Page
{
    private bool TrySetControlValueFromCookie(string cookiename, TextBox tb)
    {
        HttpCookie cookie = Request.Cookies[cookiename];
        if (cookie == null) 
            return false;
        tb.Text = cookie.Value;
        return true;
    }

    private bool TrySetControlValueFromCookie(string cookiename, SelectDateControl sd)
    {
        HttpCookie cookie = Request.Cookies[cookiename];
        if (cookie == null) 
            return false;
        DateTime dt;
        if (! DateTime.TryParse(cookie.Value, out dt))
            return false;
        sd.SelectedDate = dt;
        return true;
    }

    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(inputCreateOpenId);

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["betapassword"]))
                inputBetaPassword.Attributes["value"] = Request["betapassword"];

            if (!string.IsNullOrEmpty(Request["ConsumerUrl"]))
            {
                inputOpenId.Text = Request["ConsumerUrl"];
                inputOpenId.Enabled = false;
            }

            panelBeta.Visible = SessionManager.AccountService.IsBetaPasswordSet();

            linkAdministrator.OnClientClick =
                string.Format("location.href='mailto:{0}';",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Admin.EmailAddress", "admin@localhost.com"));

            TrySetControlValueFromCookie("SnCore.AccountCreate.Name", inputName);
            TrySetControlValueFromCookie("SnCore.AccountCreate.BetaPassword", inputBetaPassword);
            TrySetControlValueFromCookie("SnCore.AccountCreate.Birthday", inputBirthday);
            TrySetControlValueFromCookie("SnCore.AccountCreate.Email", inputEmailAddress);

            string openidmode = Request["openid.mode"];
            if (!string.IsNullOrEmpty(openidmode))
            {
                NameValueCollectionSerializer serializer = new NameValueCollectionSerializer(Request.Params);
                
                TransitAccount ta = new TransitAccount();
                ta.Name = inputName.Text;
                ta.Birthday = inputBirthday.SelectedDate;
 
                int id = SessionManager.AccountService.CreateAccountWithOpenId(
                    inputBetaPassword.Text.Trim(),
                    SessionManager.OpenIdToken,
                    serializer.Names,
                    serializer.Values,
                    inputEmailAddress.Text.Trim(),
                    ta);

                string ticket = ManagedAccount.GetTicketFromAccountId(id);
                SessionManager.Login(ticket, false);
                Redirect("AccountCreateWelcome.aspx");
            }

            if (SessionManager.IsLoggedIn)
            {
                ReportInfo("You're already logged-in. Click <a href='AccountManage.aspx'>here</a> to continue.");
                panelCreateOpenId.Enabled = false;
                return;
            }
        }
    }

    protected void CreateOpenId_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(inputName.Text))
        {
            throw new ArgumentException("Please enter your name.");
        }

        if (string.IsNullOrEmpty(inputOpenId.Text))
        {
            throw new ArgumentException("Please enter your open-id.");
        }

        if (string.IsNullOrEmpty(inputEmailAddress.Text))
        {
            throw new ArgumentException("Please enter an e-mail address.");
        }

        if (!inputBirthday.HasDate)
        {
            throw new ArgumentException("Please enter a valid date of birth.");
        }

        // url root needs to be a case-sensitive match for the openid server trust
        TransitOpenIdRedirect redirect = SessionManager.AccountService.GetOpenIdRedirect(inputOpenId.Text, Request.Url.ToString());
        SessionManager.OpenIdToken = redirect.Token;
        Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.Name", inputName.Text));
        Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.BetaPassword", inputBetaPassword.Text.Trim()));
        Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.Email", inputEmailAddress.Text.Trim()));
        Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.Birthday", inputBirthday.SelectedDate.ToString()));
        Redirect(redirect.Url);
    }
}
