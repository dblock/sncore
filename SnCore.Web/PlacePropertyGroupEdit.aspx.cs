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
using SnCore.WebServices;
using SnCore.Services;
using System.Text;

public partial class PlacePropertyGroupEdit : AuthenticatedPage
{
    public int PlaceId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(save);

            if (!IsPostBack)
            {
                linkBack.NavigateUrl = string.Format("PlaceEdit.aspx?id={0}", PlaceId);

                TransitPlace tp = PlaceService.GetPlaceById(PlaceId);

                if (RequestId > 0)
                {
                    TransitPlacePropertyGroup tag = PlaceService.GetPlacePropertyGroupById(RequestId);
                    labelName.Text = string.Format("{0}: {1}", Render(tp.Name), Render(tag.Name));
                    labelDescription.Text = Render(tag.Description);
                }
                else
                {
                    labelName.Text = string.Format("{0}: All Property Groups", Render(tp.Name));
                }

                gridManage.DataSource = PlaceService.GetAllPlacePropertyValuesById(PlaceId, RequestId);
                gridManage.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (DataGridItem item in gridManage.Items)
            {
                switch (item.ItemType)
                {
                    case ListItemType.Item:
                    case ListItemType.AlternatingItem:
                    case ListItemType.SelectedItem:
                        int id = int.Parse(((HiddenField)item.FindControl("Id")).Value);
                        int property_id = int.Parse(((HiddenField)item.FindControl("propertyId")).Value);

                        TransitPlaceProperty prop = PlaceService.GetPlacePropertyById(property_id);

                        TransitPlacePropertyValue value = new TransitPlacePropertyValue();
                        value.Id = id;
                        value.PlaceId = PlaceId;
                        value.PlaceProperty = prop;
                        switch (prop.Type.ToString())
                        {
                            case "System.Array":
                                value.Value = StringToArray(((TextBox)item.FindControl("array_value")).Text);
                                break;
                            case "System.Text.StringBuilder":
                                value.Value = ((TextBox)item.FindControl("text_value")).Text;
                                break;
                            case "System.String":
                                value.Value = ((TextBox)item.FindControl("string_value")).Text;
                                break;
                            case "System.Int32":
                                value.Value = ((TextBox)item.FindControl("int_value")).Text;
                                break;
                            case "System.Boolean":
                                value.Value = ((CheckBox)item.FindControl("bool_value")).Checked.ToString();
                                break;
                        }

                        value.Id = PlaceService.CreateOrUpdatePlacePropertyValue(
                            SessionManager.Ticket, value);
                        break;
                }
            }
            
            Redirect(linkBack.NavigateUrl);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private string StringToArray(string value)
    {
        string[] arr = value.Split(",;".ToCharArray());
        if (arr.Length == 0) return string.Empty;
        StringBuilder sb = new StringBuilder(value.Length);
        foreach (string s in arr)
        {
            sb.Append("\"" + s.Trim() + "\"");
        }
        return sb.ToString();
    }
}
