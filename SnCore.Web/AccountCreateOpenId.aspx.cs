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

[SiteMapDataAttribute("Join")]
public partial class AccountCreateOpenId : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(inputCreateOpenId);

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["betapassword"]))
                inputBetaPassword.Attributes["value"] = Request["betapassword"];

            panelBeta.Visible = SessionManager.AccountService.IsBetaPasswordSet();

            linkAdministrator.OnClientClick =
                string.Format("location.href='mailto:{0}';",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Admin.EmailAddress", "admin@localhost.com"));

            string openidmode = Request["openid.mode"];
            if (!string.IsNullOrEmpty(openidmode))
            {
                NameValueCollectionSerializer serializer = new NameValueCollectionSerializer(Request.Params);
                string consumerurl = SessionManager.AccountService.VerifyOpenId(SessionManager.OpenIdToken, serializer.Names, serializer.Values);

                TransitAccount ta = new TransitAccount();
                ta.Name = Request.Cookies["SnCore.AccountCreate.Name"].Value;

                int id = SessionManager.AccountService.CreateAccountWithOpenId(
                    Request.Cookies["SnCore.AccountCreate.BetaPassword"].Value,
                    consumerurl,
                    ta);

                string ticket = FormsAuthentication.GetAuthCookie(id.ToString(), false).Value;
                SessionManager.Login(ticket, false);
                Redirect("Default.aspx");
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

        // url root needs to be a case-sensitive match for the openid server trust
        TransitOpenIdRedirect redirect = SessionManager.AccountService.GetOpenIdRedirect(inputOpenId.Text, Request.Url.ToString());
        SessionManager.OpenIdToken = redirect.Token;
        Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.Name", inputName.Text));
        Response.Cookies.Add(new HttpCookie("SnCore.AccountCreate.BetaPassword", inputBetaPassword.Text.ToString()));
        Redirect(redirect.Url);
    }
}
