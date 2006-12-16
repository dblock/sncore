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
using SnCore.SiteMap;

public partial class AccountEmailVerify : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            inputCode.Text = Request.QueryString["code"];

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("E-Mails", Request, "AccountEmailsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Verify", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(inputVerify);
    }

    public void EmailVerify_Click(object sender, EventArgs e)
    {
        string emailaddress = SessionManager.AccountService.VerifyEmail(
            inputPassword.Text,
            RequestId,
            inputCode.Text);

        if (!SessionManager.IsLoggedIn)
        {
            string ticket = SessionManager.AccountService.Login(emailaddress, inputPassword.Text);
            SessionManager.Login(ticket, true);
        }

        panelVerify.Visible = false;
        ReportInfo("Thank you. Your e-mail address has been verified." +
            "<br>Click <a href='Default.aspx'>here</a> to continue.", false);
    }
}
