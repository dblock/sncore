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
using Facebook;
using Facebook.Web;

public partial class _Default : Facebook.Web.CanvasFBMLBasePage
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        base.RequireLogin = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
}
