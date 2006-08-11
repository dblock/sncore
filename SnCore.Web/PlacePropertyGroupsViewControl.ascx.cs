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

public partial class PlacePropertyGroupsViewControl : Control
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

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                groups.DataSource = PlaceService.GetPlacePropertyGroups();
                groups.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void groups_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        try
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                case ListItemType.SelectedItem:
                    DataGrid values = (DataGrid)e.Item.FindControl("values");
                    TransitPlacePropertyGroup group = (TransitPlacePropertyGroup) e.Item.DataItem;
                    object[] args = { PlaceId, group.Id };
                    List<TransitPlacePropertyValue> propertyvalues = SessionManager.GetCachedCollection<TransitPlacePropertyValue>(
                        PlaceService, "GetPlacePropertyValuesById", args);
                    values.DataSource = propertyvalues;
                    HtmlControl title = (HtmlControl)e.Item.FindControl("title");
                    title.Visible = (propertyvalues.Count > 0);
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }    
}
