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
using SnCore.Tools.Web;

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
                groups.DataSource = SessionManager.GetCachedCollection<TransitPlacePropertyGroup>(
                    SessionManager.PlaceService, "GetPlacePropertyGroups", null);
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
                    if (group != null)
                    {
                        object[] args = { PlaceId, group.Id };
                        List<TransitPlacePropertyValue> propertyvalues = SessionManager.GetCachedCollection<TransitPlacePropertyValue>(
                            SessionManager.PlaceService, "GetPlacePropertyValuesById", args);
                        values.DataSource = propertyvalues;
                        HtmlControl title = (HtmlControl)e.Item.FindControl("title");
                        title.Visible = (propertyvalues.Count > 0);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string RenderSystemArray(string groupname, string propname, string value)
    {
        StringBuilder sb = new StringBuilder();
        string[] arr = value.Split("\"".ToCharArray());
        foreach(string s in arr)
        {
            if (string.IsNullOrEmpty(s))
                continue;

            if (sb.Length > 0) sb.Append(", ");
            sb.AppendFormat("<a href=\"PlacesByPropertyValueView.aspx?GroupName={1}&PropertyName={2}&PropertyValue={0}\">{0}</a>",
                Renderer.Render(s), groupname, propname);
        }
        return sb.ToString();
    }

    public string RenderValue(Type type, string groupname, string propname, string value)
    {
        switch (type.ToString())
        {
            case "System.Array":
                return RenderSystemArray(groupname, propname, value);
            default:
                return RenderEx(value);
        }
    }
}
