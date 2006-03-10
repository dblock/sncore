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

public partial class AccountCreate : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(inputLogin);

            if (!IsPostBack)
            {
                linkAdministrator.OnClientClick =
                    string.Format("location.href='mailto:{0}';",
                        SystemService.GetConfigurationByNameWithDefault(
                            "SnCore.Admin.EmailAddress", "admin@localhost.com").Value);

                ArrayList countries = new ArrayList();
                countries.Add(new TransitCountry());
                countries.AddRange(LocationService.GetCountries());

                ArrayList states = new ArrayList();
                states.Add(new TransitState());

                inputCountry.DataSource = countries;
                inputCountry.DataBind();

                inputState.DataSource = states;
                inputState.DataBind();

                string openidmode = Request["openid.mode"];
                if (!string.IsNullOrEmpty(openidmode))
                {
                    string consumerurl = AccountService.VerifyOpenId(SessionManager.OpenIdToken, Request.Params);

                    TransitAccount ta = new TransitAccount();
                    ta.Name = Request.Cookies["SnCore.AccountCreate.Name"].Value;
                    ta.Country = Request.Cookies["SnCore.AccountCreate.Country"].Value;
                    ta.City = Request.Cookies["SnCore.AccountCreate.City"].Value;
                    ta.State = Request.Cookies["SnCore.AccountCreate.State"].Value;
                    ta.Birthday = DateTime.Parse(Request.Cookies["SnCore.AccountCreate.Birthday"].Value);

                    int id = AccountService.CreateAccountWithOpenId(
                        Request.Cookies["SnCore.AccountCreate.BetaPassword"].Value, 
                        consumerurl, 
                        ta);

                    string ticket = FormsAuthentication.GetAuthCookie(id.ToString(), false).Value;
                    SessionManager.Login(ticket, false);
                    Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected void create_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(inputOpenId.Text))
            {
                // url root needs to be a case-sensitive match for the openid server trust
                TransitOpenIdRedirect redirect = AccountService.GetOpenIdRedirect(inputOpenId.Text, Request.Url.ToString());
                SessionManager.OpenIdToken = redirect.Token;
                Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.Name", inputName.Text));
                Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.Country", inputCountry.Text));
                Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.City", inputCity.Text));
                Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.State", inputState.Text));
                Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.Birthday", inputBirthday.SelectedDate.ToString()));
                Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.BetaPassword", inputBetaPassword.Text.ToString()));
                Redirect(redirect.Url);
                return;
            }

            if (inputPassword.Text != inputPassword2.Text)
            {
                throw new ArgumentException("Passwords don't match.");
            }

            TransitAccount ta = new TransitAccount();
            ta.Name = inputName.Text;
            ta.Country = inputCountry.SelectedValue;
            ta.City = inputCity.Text;
            ta.State = inputState.SelectedValue;
            ta.Birthday = inputBirthday.SelectedDate;

            // check whether there's already an account with the same e-mail and password
            try
            {
                string ticket = AccountService.Login(inputEmailAddress.Text, inputPassword.Text);
                ReportWarning("There's already an account with the same e-mail address and password.");
                return;
            }
            catch
            {
            }

            AccountService.CreateAccount(
                inputBetaPassword.Text,
                inputPassword.Text,
                inputEmailAddress.Text,
                ta);

            try
            {
                string ticket = AccountService.Login(inputEmailAddress.Text, inputPassword.Text);
                SessionManager.Login(ticket, false);
                Redirect("Default.aspx");
            }
            catch
            {
                // there's a confirmed e-mail address, this one cannot be used to login immediately

                ReportInfo(
                    string.Format(
                        "Account created. An e-mail has been sent to '{0}' for confirmation. " +
                        "You must confirm this e-mail address before you can use it to login.",
                        inputEmailAddress.Text));

                panelCreate.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            inputPassword.Attributes.Add("value", inputPassword.Text);
            inputPassword2.Attributes.Add("value", inputPassword2.Text);
            inputBetaPassword.Attributes.Add("value", inputBetaPassword.Text);

            ArrayList states = new ArrayList();
            states.Add(new TransitState());
            states.AddRange(LocationService.GetStatesByCountry(inputCountry.SelectedValue));

            inputState.DataSource = states;
            inputState.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
