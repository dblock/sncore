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

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkLoginLogout.Text = SessionManager.IsLoggedIn ? "Logout" : "Login";            
        }
    }

    public void linkLoginLogout_Click(object sender, EventArgs e)
    {
        if (SessionManager.IsLoggedIn)
        {
            SessionManager.Logout();
            Redirect("Default.aspx");
        }
        else
        {
            Redirect("AccountLogin.aspx");
        }
    }
}
