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

public partial class AccountPropertyGroupEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(save);

            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitAccountPropertyGroup tag = AccountService.GetAccountPropertyGroupById(RequestId);
                    labelName.Text = Render(tag.Name);
                    labelDescription.Text = Render(tag.Description);

                }
                else
                {
                    labelName.Text = "All Property Groups";
                }

                gridManage.DataSource = AccountService.GetAllAccountPropertyValues(SessionManager.Ticket, RequestId);
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

                        TransitAccountProperty prop = AccountService.GetAccountPropertyById(property_id);

                        TransitAccountPropertyValue value = new TransitAccountPropertyValue();
                        value.Id = id;
                        value.AccountId = SessionManager.Account.Id;
                        value.AccountProperty = prop;
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

                        value.Id = AccountService.CreateOrUpdateAccountPropertyValue(
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
