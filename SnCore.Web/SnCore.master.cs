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
using SnCore.Tools.Web;
using nStuff.UpdateControls;

public partial class SnCoreMasterPage : MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Page.Title = string.Format("{0} - {1}", SessionManager.GetCachedConfiguration(
                    "SnCore.Title", "SnCore"), Page.Title);

                panelAds.Visible = SessionManager.ShowAds;
                panelCounter.Visible = SessionManager.ShowCounter;

                if (Request.PhysicalPath.EndsWith("Default.aspx"))
                {
                    siteMapPath.Visible = false;
                }

                GetMobileWeb(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private void GetMobileWeb(object sender, EventArgs e)
    {
        if (Request.Browser.IsMobileDevice)
        {
            if (!string.IsNullOrEmpty(SessionManager.MobileWebsiteUrl))
            {
                string mobileweb = SessionManager.MobileWebsiteUrl;
                if (((Page)Page).IsMobileEnabled)
                {
                    mobileweb = Request.Url.ToString().Replace(
                        SessionManager.WebsiteUrl, SessionManager.MobileWebsiteUrl);
                }

                linkMobile.NavigateUrl = mobileweb;
                panelMobile.Visible = true;
            }
        }
    }

    public override void OnPagePreInit(EventArgs e)
    {
        if (Request.Browser.Crawler || Request.Browser.W3CDomVersion.Major < 1)
        {
            scriptmanager1.EnablePartialRendering = false;
        }

        base.OnPagePreInit(e);
    }

    public string ReturnUrl
    {
        get
        {
            string returnurl = Request["ReturnUrl"];
            if (string.IsNullOrEmpty(returnurl)) returnurl = "Default.aspx";
            return returnurl;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            menuMe.Visible = SessionManager.IsLoggedIn; 
            menuLogout.Visible = SessionManager.IsLoggedIn;
            menuSignUp.Visible = !SessionManager.IsLoggedIn;
            menuInvite.Visible = SessionManager.IsLoggedIn;
            menuLogin.Visible = !SessionManager.IsLoggedIn;
            
            menuLogin.NavigateUrl = string.Format("AccountLogin.aspx?ReturnUrl={0}",
                Renderer.UrlEncode(Request.Url.PathAndQuery.Contains("/AccountLogin.aspx") 
                    ? ReturnUrl : Request.Url.PathAndQuery));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

        base.OnPreRender(e);
    }

    protected void menuLogout_Click(object sender, EventArgs e)
    {
        SessionManager.Logout();
        Redirect("Default.aspx");
    }

    public UpdateHistory History
    {
        get
        {
            return updateHistory;
        }
    }
}
