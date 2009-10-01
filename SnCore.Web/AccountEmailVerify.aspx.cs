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
            inputId.Text = (RequestId == 0) 
                ? string.Empty 
                : RequestId.ToString();
                    
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
        int id = RequestId;
        if (!int.TryParse(inputId.Text, out id))
        {
            throw new Exception(string.Format("Confirmation id '{0}' is not a valid number.", 
                inputId.Text));
        }

        string emailaddress = SessionManager.AccountService.VerifyAccountEmail(
            id,  inputCode.Text);

        panelVerify.Visible = false;
        ReportInfo(string.Format("Thank you. Your e-mail address '{0}' has been verified." +
            "<br>Click <a href='AccountManage.aspx'>here</a> to continue.", Renderer.Render(emailaddress)), false);

        SessionManager.InvalidateCache<TransitAccount>();
    }

    protected override void OnPreRender(EventArgs e)
    {
        inputId.Enabled = (RequestId <= 0);
        inputCode.Enabled = (string.IsNullOrEmpty(Request.QueryString["code"]));
        base.OnPreRender(e);
    }
}
