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
using SnCore.SiteMap;

[SiteMapDataAttribute("Join")]
public partial class AccountCreate : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(inputCreate);

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["betapassword"]))
                inputBetaPassword.Attributes["value"] = Request["betapassword"];

            panelBeta.Visible = SessionManager.AccountService.IsBetaPasswordSet();

            linkAdministrator.OnClientClick =
                string.Format("location.href='mailto:{0}';",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Admin.EmailAddress", "admin@localhost.com"));
        }
        else
        {
            inputBetaPassword.Attributes["value"] = inputBetaPassword.Text.Trim();
            inputPassword.Attributes["value"] = inputPassword.Text.Trim();
            inputPassword2.Attributes["value"] = inputPassword2.Text.Trim();
        }
    }

    protected void create_Click(object sender, EventArgs e)
    {
        inputBetaPassword.Attributes["value"] = inputBetaPassword.Text.Trim();
        inputPassword.Attributes["value"] = inputPassword.Text.Trim();
        inputPassword2.Attributes["value"] = inputPassword2.Text.Trim();

        if (string.IsNullOrEmpty(inputName.Text))
        {
            throw new ArgumentException("Please enter a name.");
        }

        if (inputPassword.Text.Trim() != inputPassword2.Text.Trim())
        {
            throw new ArgumentException("Passwords don't match.");
        }

        if (!inputBirthday.HasDate)
        {
            throw new ArgumentException("Please select a valid date.");
        }

        if (string.IsNullOrEmpty(inputPassword.Text.Trim()))
        {
            throw new ArgumentException("Please enter a password.");
        }

        TransitAccount ta = new TransitAccount();
        ta.Name = inputName.Text;
        ta.Birthday = inputBirthday.SelectedDate;
        ta.Password = inputPassword.Text.Trim();

        try
        {
            // check whether there's already an account with the same e-mail and password
            string ticket = SessionManager.AccountService.Login(inputEmailAddress.Text.Trim(), inputPassword.Text.Trim());
            ReportWarning("There's already an account with the same e-mail address and password.");
            return;
        }
        catch
        {
        }

        try
        {
            SessionManager.AccountService.CreateAccount(
                inputBetaPassword.Text.Trim(),
                inputEmailAddress.Text.Trim(),
                ta);

            string ticket = SessionManager.AccountService.Login(inputEmailAddress.Text.Trim(), inputPassword.Text.Trim());
            SessionManager.Login(ticket, false);
            Redirect("AccountCreateWelcome.aspx");
        }
        catch
        {
            // there's a confirmed e-mail address, this one cannot be used to login immediately

            ReportInfo(
                string.Format(
                    "Account created. An e-mail was sent to '{0}' for confirmation. " +
                    "You must confirm this e-mail address before you log-in.",
                    inputEmailAddress.Text.Trim()));

            panelCreate.Visible = false;
        }
    }
}
