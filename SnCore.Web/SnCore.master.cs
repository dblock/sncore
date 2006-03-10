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

    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            menuMe.Visible = SessionManager.IsLoggedIn; 
            menuLogin.Visible = !SessionManager.IsLoggedIn;
            menuLogout.Visible = SessionManager.IsLoggedIn;
            menuSignUp.Visible = !SessionManager.IsLoggedIn;
            menuInvite.Visible = SessionManager.IsLoggedIn;
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
