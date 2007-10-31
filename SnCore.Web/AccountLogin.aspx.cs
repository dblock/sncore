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
using SnCore.SiteMap;

[SiteMapDataAttribute("Login")]
public partial class AccountLogin : Page
{
    public AccountLogin()
    {
        mIsMobileEnabled = true;
    }

    public bool AuthenticatedPage
    {
        get
        {
            bool result = false;
            object ap = Request.Params["AuthenticatedPage"];
            if (ap == null) return false;
            bool.TryParse(ap.ToString(), out result);
            return result;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (AuthenticatedPage)
            {
                ReportInfo("This is a members only page. You must first login. There're many perks of being a member " +
                    "and it's free. Click <a href=\"AccountCreate.aspx\">here</a> to join!");
            }
        }
    }
}
