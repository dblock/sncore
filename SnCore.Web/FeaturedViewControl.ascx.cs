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
using SnCore.Services;
using System.Collections.Generic;
using SnCore.WebServices;

public partial class FeaturedViewControl : Control
{
    public void Page_Load()
    {

    }

    public string GetLinkMailToAdministrator()
    {
        return string.Format("location.href='mailto:{0}';",
            SessionManager.GetCachedConfiguration(
                "SnCore.Admin.EmailAddress", "admin@localhost.com"));
    }
}
