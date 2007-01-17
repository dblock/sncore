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
using Wilco.Web.UI;
using SnCore.Services;
using System.Collections.Generic;
using System.Text;
using SnCore.WebServices;

public partial class PlacePropertyValuesViewControl : Control
{
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

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            values.DataSource = SessionManager.GetCollection<TransitDistinctPlacePropertyValue, string, string>(
                GroupName, PropertyName, (ServiceQueryOptions) null, SessionManager.PlaceService.GetDistinctPropertyValues);
            values.DataBind();
        }
    }
}
