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
            inputBetaPassword.Attributes["value"] = inputBetaPassword.Text;
            inputPassword.Attributes["value"] = inputPassword.Text;
            inputPassword2.Attributes["value"] = inputPassword2.Text;
        }
    }

    protected void create_Click(object sender, EventArgs e)
    {
        inputBetaPassword.Attributes["value"] = inputBetaPassword.Text;
        inputPassword.Attributes["value"] = inputPassword.Text;
        inputPassword2.Attributes["value"] = inputPassword2.Text;

        if (string.IsNullOrEmpty(inputName.Text))
        {
            throw new ArgumentException("Please enter a name.");
        }

        if (inputPassword.Text != inputPassword2.Text)
        {
            throw new ArgumentException("Passwords don't match.");
        }

        if (!inputBirthday.HasDate)
        {
            throw new ArgumentException("Please select a valid date.");
        }

        if (string.IsNullOrEmpty(inputPassword.Text))
        {
            throw new ArgumentException("Please enter a password.");
        }

        TransitAccount ta = new TransitAccount();
        ta.Name = inputName.Text;
        ta.Birthday = inputBirthday.SelectedDate;

        try
        {
            // check whether there's already an account with the same e-mail and password
            string ticket = SessionManager.AccountService.Login(inputEmailAddress.Text, inputPassword.Text);
            ReportWarning("There's already an account with the same e-mail address and password.");
            return;
        }
        catch
        {
        }

        try
        {
            SessionManager.AccountService.CreateAccount(
                inputBetaPassword.Text,
                inputPassword.Text,
                inputEmailAddress.Text,
                ta);

            string ticket = SessionManager.AccountService.Login(inputEmailAddress.Text, inputPassword.Text);
            SessionManager.Login(ticket, false);
            Redirect("AccountCreateWelcome.aspx");
        }
        catch
        {
            // there's a confirmed e-mail address, this one cannot be used to login immediately

            ReportInfo(
                string.Format(
                    "Account created. An e-mail has been sent to '{0}' for confirmation. " +
                    "You must confirm this e-mail address before you log-in.",
                    inputEmailAddress.Text));

            panelCreate.Visible = false;
        }
    }
}
