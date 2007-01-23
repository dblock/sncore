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
using SnCore.WebServices;

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
        if (!IsPostBack)
        {
            groups.DataSource = SessionManager.GetCollection<TransitPlacePropertyGroup>(
                null, SessionManager.PlaceService.GetPlacePropertyGroups);
            groups.DataBind();
        }
    }

    public void groups_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
            case ListItemType.SelectedItem:
                DataGrid values = (DataGrid)e.Item.FindControl("values");
                TransitPlacePropertyGroup group = (TransitPlacePropertyGroup)e.Item.DataItem;
                if (group != null)
                {
                    IList<TransitPlacePropertyValue> propertyvalues = SessionManager.GetCollection<TransitPlacePropertyValue, int, int>(
                        PlaceId, group.Id, null, SessionManager.PlaceService.GetPlacePropertyValues);

                    for (int i = propertyvalues.Count - 1; i >= 0; i--)
                    {
                        if (string.IsNullOrEmpty(propertyvalues[i].Value))
                        {
                            propertyvalues.RemoveAt(i);
                        }
                    }

                    if (propertyvalues.Count == 0) values.Visible = false;
                    values.DataSource = propertyvalues;
                    HtmlControl title = (HtmlControl)e.Item.FindControl("title");
                    title.Visible = (propertyvalues.Count > 0);
                }
                break;
        }
    }

    public string RenderSystemArray(string groupname, string propname, string value)
    {
        StringBuilder sb = new StringBuilder();
        string[] arr = value.Split("\"".ToCharArray());
        foreach (string s in arr)
        {
            if (string.IsNullOrEmpty(s))
                continue;

            if (sb.Length > 0) sb.Append(", ");
            sb.AppendFormat("<a href=\"PlacesByPropertyValueView.aspx?GroupName={1}&PropertyName={2}&PropertyValue={0}\">{0}</a>",
                Renderer.Render(s), groupname, propname);
        }
        return sb.ToString();
    }

    public string RenderValue(string type, string groupname, string propname, string value)
    {
        switch (type)
        {
            case "System.Array":
                return RenderSystemArray(groupname, propname, value);
            default:
                return RenderEx(value);
        }
    }
}
