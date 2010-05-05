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

public partial class FacebookConnect : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string returnUrl = Request["ReturnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
                throw new Exception("Missing ReturnUrl");
            returnUrl += (returnUrl.IndexOf('?') > 0) ? "&connect=1" : "?connect=1";
            Response.Redirect(returnUrl);
        }
    }
}
