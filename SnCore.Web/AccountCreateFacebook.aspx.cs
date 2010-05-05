using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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

[SiteMapDataAttribute("Join with Facebook")]
public partial class AccountCreateFacebook : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(inputCreateFacebook);

        if (!IsPostBack)
        {
            FacebookPageManager facebook = new FacebookPageManager(SessionManager);
            if (string.IsNullOrEmpty(facebook.FacebookAPIKey))
            {
                throw new Exception("This site is not configured for Facebook login: missing Facebook API key.");
            }

            if (!string.IsNullOrEmpty(Request["betapassword"]))
                inputBetaPassword.Attributes["value"] = Request["betapassword"];

            panelBeta.Visible = SessionManager.AccountService.IsBetaPasswordSet();

            linkAdministrator.OnClientClick =
                string.Format("location.href='mailto:{0}';",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Admin.EmailAddress", "admin@localhost.com"));

            panelFacebook.Enabled = true;
            panelIdentity.Enabled = false;

            string facebooklogin = Request["facebook.login"];
            if (!string.IsNullOrEmpty(facebooklogin))
            {
                SortedList<string, string> facebookCookies = facebook.GetFacebookCookies(HttpContext.Current.Request.Cookies);
                List<String> keys = new List<String>(facebookCookies.Keys);
                List<String> values = new List<String>(facebookCookies.Values);
                TransitFacebookLogin t_login = SessionManager.AccountService.TryLoginFacebook(
                    HttpContext.Current.Request.Cookies[facebook.FacebookAPIKey].Value,
                    keys.ToArray(), values.ToArray());

                if (t_login != null)
                {
                    throw new Exception(string.Format("An account for the Facebook user id \"{0}\" already exists. Please log-in instead.",
                        facebookCookies["user"]));
                }

                panelIdentity.Enabled = true;
                panelFacebook.Enabled = false;
            }

            if (SessionManager.IsLoggedIn)
            {
                ReportInfo("You're already logged-in. Click <a href='AccountManage.aspx'>here</a> to continue.");
                panelCreateFacebook.Enabled = false;
                return;
            }
        }
    }

    protected void CreateFacebook_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(inputName.Text))
        {
            throw new ArgumentException("Please enter your name.");
        }

        if (string.IsNullOrEmpty(inputEmailAddress.Text))
        {
            throw new ArgumentException("Please enter an e-mail address.");
        }

        if (!inputBirthday.HasDate)
        {
            throw new ArgumentException("Please enter a valid date of birth.");
        }

        FacebookPageManager facebook = new FacebookPageManager(SessionManager);
        SortedList<string, string> facebookCookies = facebook.GetFacebookCookies(HttpContext.Current.Request.Cookies);
        if (facebookCookies.Count == 0)
            throw new Exception("Missing Facebook cookies");

        TransitAccount ta = new TransitAccount();
        ta.Name = inputName.Text;
        ta.Birthday = inputBirthday.SelectedDate;

        List<String> keys = new List<String>(facebookCookies.Keys);
        List<String> values = new List<String>(facebookCookies.Values);
        int id = SessionManager.AccountService.CreateAccountWithFacebook(
            inputBetaPassword.Text,
            HttpContext.Current.Request.Cookies[facebook.FacebookAPIKey].Value, 
            keys.ToArray(), 
            values.ToArray(),
            inputEmailAddress.Text,
            ta);

        string ticket = ManagedAccount.GetTicketFromAccountId(id);
        SessionManager.Login(ticket, false);
        Redirect("AccountCreateWelcome.aspx");
    }
}
