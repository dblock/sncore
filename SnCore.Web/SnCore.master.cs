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

public partial class SnCoreMasterPage : MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                panelAds.Visible = SessionManager.ShowAds;
                panelCounter.Visible = SessionManager.ShowCounter;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
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
}
