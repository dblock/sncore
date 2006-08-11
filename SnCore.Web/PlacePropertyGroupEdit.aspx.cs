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

                TransitPlacePropertyGroup tag = PlaceService.GetPlacePropertyGroupById(RequestId);
                labelName.Text = Render(tag.Name);
                labelDescription.Text = Render(tag.Description);

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
                            case "System.String":
                                value.Value = ((TextBox)item.FindControl("text_value")).Text;
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
}
