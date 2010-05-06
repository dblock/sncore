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

    }

    public string ReturnUrl
    {
        get
        {
            string returnUrl = Request["ReturnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new Exception("Missing ReturnUrl");
            }

            if (returnUrl.IndexOf("connect=1") < 0)
            {
                returnUrl += (returnUrl.IndexOf('?') > 0) ? "&connect=1" : "?connect=1";
            }

            return returnUrl;
        }
    }
}
