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
using SnCore.Services;
using SnCore.Tools.Web;

public partial class AccountEmailVerify : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            inputCode.Text = Request.QueryString["code"];

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("E-Mails", Request, "AccountEmailsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Verify", Request.Url));
            StackSiteMap(sitemapdata);

            if (! string.IsNullOrEmpty(inputCode.Text))
            {
                EmailVerify_Click(sender, e);
            }
        }

        SetDefaultButton(inputVerify);
    }

    public void EmailVerify_Click(object sender, EventArgs e)
    {
        string emailaddress = SessionManager.AccountService.VerifyAccountEmail(
            RequestId,
            inputCode.Text);

        panelVerify.Visible = false;
        ReportInfo(string.Format("Thank you. Your e-mail address '{0}' has been verified." +
            "<br>Click <a href='AccountManage.aspx'>here</a> to continue.", Renderer.Render(emailaddress)), false);

        SessionManager.InvalidateCache<TransitAccount>();
    }
}
