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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Generic;
using nStuff.UpdateControls;
using System.Text;
using System.Collections.Specialized;

public partial class AccountCityControl : Control
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!SessionManager.IsLoggedIn)
                return;

            if (SessionManager.Account == null || string.IsNullOrEmpty(SessionManager.Account.City))
                return;

            welcome.Visible = true;            
            welcome.Text = string.Format("<b>{0}</b> Community", SessionManager.Account.City);

            if (!Request.PhysicalPath.EndsWith("Default.aspx"))
            {
                welcome.NavigateUrl = "Default.aspx";
            }
            else
            {
                change.Visible = true;
            }
        }
    }
}
