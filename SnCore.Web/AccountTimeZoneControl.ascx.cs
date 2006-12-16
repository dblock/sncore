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
using SnCore.Tools;

public partial class AccountTimeZoneControl : Control
{
    public string CssClass
    {
        get
        {
            return tableMain.Attributes["class"];
        }
        set
        {
            tableMain.Attributes["class"] = value;
        }
    }

    public string Width
    {
        get
        {
            return tableMain.Width;
        }
        set
        {
            tableMain.Width = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SessionManager.IsLoggedIn && SessionManager.Account.TimeZone >= 0)
            {
                TimeZoneInformation tz = TimeZoneInformation.FromIndex(SessionManager.Account.TimeZone);
                labelTimeZone.Text = tz.DisplayName;
            }
            else
            {
                labelTimeZone.Text = string.Format("UTC {0} (Browser TimeZone)", SessionManager.BrowserUtcOffset);
            }
        }
    }
}
