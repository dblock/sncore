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
using SnCore.Tools.Web;
using SnCore.WebControls;
using Wilco.Web.UI;
using SnCore.Services;

public partial class PlacePropertyValueViewControl : Control
{
    public int PlaceId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "PlaceId", 0);
        }
        set
        {
            ViewState["PlaceId"] = value;
        }
    }

    public string GroupName
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "GroupName", string.Empty);
        }
        set
        {
            ViewState["GroupName"] = value;
        }
    }

    public string PropertyName
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "PropertyName", string.Empty);
        }
        set
        {
            ViewState["PropertyName"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            TransitPlacePropertyValue tppv = PlaceService.GetPlacePropertyValueByName(
                PlaceId, GroupName, PropertyName);

            labelValue.Text = RenderEx(tppv.Value);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
