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
using SnCore.Tools.Web;
using SnCore.Services;

public partial class SystemAccountPropertyEdit : AuthenticatedPage
{
    public int PropertyGroupId
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
            SetDefaultButton(manageAdd);

            if (!IsPostBack)
            {
                linkBack.NavigateUrl = string.Format("SystemAccountPropertyGroupEdit.aspx?id={0}", PropertyGroupId);

                inputTypeName.Items.Add(new ListItem(Type.GetType("System.Int32").ToString()));
                inputTypeName.Items.Add(new ListItem(Type.GetType("System.Boolean").ToString()));
                inputTypeName.Items.Add(new ListItem(Type.GetType("System.String").ToString()));
                // inputTypeName.Items.Add(new ListItem(Type.GetType("System.DateTime").ToString()));
                    
                if (RequestId > 0)
                {
                    TransitAccountProperty t = AccountService.GetAccountPropertyById(RequestId);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    inputDefaultValue.Text = t.DefaultValue;
                    inputPublish.Checked = t.Publish;

                    ListItem typeitem = inputTypeName.Items.FindByValue(t.Type.ToString());
                    
                    if (typeitem == null)
                    {
                        typeitem = new ListItem(t.Type.ToString());
                        inputTypeName.Items.Add(typeitem);
                    }

                    typeitem.Selected = true;
                }
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
            TransitAccountProperty t = new TransitAccountProperty();
            t.Name = inputName.Text;
            t.Type = Type.GetType(inputTypeName.SelectedValue);
            t.Description = inputDescription.Text;
            t.DefaultValue = inputDefaultValue.Text;
            t.AccountPropertyGroupId = PropertyGroupId;
            t.Publish = inputPublish.Checked;
            t.Id = RequestId;
            AccountService.CreateOrUpdateAccountProperty(SessionManager.Ticket, t);
            Redirect(linkBack.NavigateUrl);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
