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

public partial class SelectTimezoneControl : System.Web.UI.UserControl
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TimeZoneInformation[] zones = TimeZoneInformation.EnumZones();
            selectTimezoneControl.Items.Add(new ListItem("Web Browser TimeZone", "-1"));
            foreach (TimeZoneInformation tz in zones)
            {
                selectTimezoneControl.Items.Add(new ListItem(tz.DisplayName, tz.Index.ToString()));
            }
        }
    }

    public int SelectedTzIndex
    {
        get
        {
            return int.Parse(selectTimezoneControl.SelectedValue);
        }
        set
        {
            selectTimezoneControl.SelectedValue = value.ToString();
        }
    }
}
