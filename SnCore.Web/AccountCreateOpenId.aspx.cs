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

public partial class AccountCreateOpenId : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(inputCreateOpenId);

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["betapassword"]))
                    inputBetaPassword.Attributes["value"] = Request["betapassword"];

                panelBeta.Visible = AccountService.IsBetaPasswordSet();

                linkAdministrator.OnClientClick =
                    string.Format("location.href='mailto:{0}';",
                       SessionManager.GetCachedConfiguration(
                            "SnCore.Admin.EmailAddress", "admin@localhost.com"));

                string openidmode = Request["openid.mode"];
                if (!string.IsNullOrEmpty(openidmode))
                {
                    string consumerurl = AccountService.VerifyOpenId(SessionManager.OpenIdToken, Request.Params);

                    TransitAccount ta = new TransitAccount();
                    ta.Name = Request.Cookies["SnCore.AccountCreate.Name"].Value;

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

    protected void CreateOpenId_Click(object sender, EventArgs e)
    {
        try
        {
            // url root needs to be a case-sensitive match for the openid server trust
            TransitOpenIdRedirect redirect = AccountService.GetOpenIdRedirect(inputOpenId.Text, Request.Url.ToString());
            SessionManager.OpenIdToken = redirect.Token;
            Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.Name", inputName.Text));
            Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.BetaPassword", inputBetaPassword.Text.ToString()));
            Redirect(redirect.Url);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
