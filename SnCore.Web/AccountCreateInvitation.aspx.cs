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
using SnCore.WebServices;

public partial class AccountCreateInvitation : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(inputLogin);

            if (!IsPostBack)
            {
                ArrayList countries = new ArrayList();
                countries.Add(new TransitCountry());
                countries.AddRange(LocationService.GetCountries());

                ArrayList states = new ArrayList();
                states.Add(new TransitState());

                inputCountry.DataSource = countries;
                inputCountry.DataBind();

                inputState.DataSource = states;
                inputState.DataBind();

                TransitAccountInvitation invitation = AccountService.GetAccountInvitationById(
                    SessionManager.Ticket, RequestId);

                inputEmailAddress.Text = invitation.Email;
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

            AccountService.CreateAccountInvitation(
                RequestId,
                Request.QueryString["code"],
                inputPassword.Text,
                inputEmailAddress.Text,
                ta);

            panelCreate.Visible = false;

            string ticket = AccountService.Login(inputEmailAddress.Text, inputPassword.Text);
            SessionManager.Login(ticket, false);
            Redirect("Default.aspx");
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
