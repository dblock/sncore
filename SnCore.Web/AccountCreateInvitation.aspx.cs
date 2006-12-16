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
using SnCore.SiteMap;

[SiteMapDataAttribute("Join")]
public partial class AccountCreateInvitation : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(inputLogin);

        if (!IsPostBack)
        {
            TransitAccountInvitation invitation = SessionManager.AccountService.GetAccountInvitationById(
                SessionManager.Ticket, RequestId);

            inputEmailAddress.Text = invitation.Email;
        }
    }

    protected void create_Click(object sender, EventArgs e)
    {
        if (inputPassword.Text != inputPassword2.Text)
        {
            throw new ArgumentException("Passwords don't match.");
        }

        TransitAccount ta = new TransitAccount();
        ta.Name = inputName.Text;
        ta.Birthday = inputBirthday.SelectedDate;

        SessionManager.AccountService.CreateAccountInvitation(
            RequestId,
            Request.QueryString["code"],
            inputPassword.Text,
            inputEmailAddress.Text,
            ta);

        panelCreate.Visible = false;

        string ticket = SessionManager.AccountService.Login(inputEmailAddress.Text, inputPassword.Text);
        SessionManager.Login(ticket, false);
        Redirect("AccountCreateInvitationWelcome.aspx");
    }
}
