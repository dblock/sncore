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
using SnCore.WebServices;
using System.Collections.Generic;

public partial class AccountAttributeEdit : AuthenticatedPage
{
    public int AccountId
    {
        get
        {
            return GetId("aid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                linkBack.NavigateUrl = string.Format("AccountAttributesManage.aspx?id={0}", AccountId);

                List<TransitAttribute> attributes = SystemService.GetAttributes();
                
                if (RequestId == 0)
                {
                    attributes.Insert(0, new TransitAttribute());
                }

                listAttributes.DataSource = attributes;
                listAttributes.DataBind();

                if (RequestId > 0)
                {
                    TransitAccountAttribute attribute = AccountService.GetAccountAttributeById(RequestId);
                    inputValue.Text = Renderer.Render(attribute.Value);
                    inputUrl.Text = Renderer.Render(attribute.Url);
                    inputDefaultUrl.Text = Renderer.Render(attribute.Attribute.DefaultUrl);
                    inputDefaultValue.Text = Renderer.Render(attribute.Attribute.DefaultValue);
                    listAttributes.Items.FindByValue(attribute.AttributeId.ToString()).Selected = true;
                    listAttributes.Enabled = false;
                    previewImage.ImageUrl = string.Format("SystemAttribute.aspx?id={0}", attribute.AttributeId);
                    previewImage.Visible = true;
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
            TransitAccountAttribute attribute = new TransitAccountAttribute();

            attribute.Url = inputUrl.Text;
            attribute.Value = inputValue.Text;
            attribute.Id = RequestId;
            attribute.AttributeId = int.Parse(listAttributes.SelectedValue);
            attribute.AccountId = AccountId;
            AccountService.CreateOrUpdateAccountAttribute(SessionManager.Ticket, attribute);
            Redirect(linkBack.NavigateUrl);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }

    public void listAttributes_SelectedItemChanged(object sender, EventArgs e)
    {
        try
        {
            int id = 0;
            if (int.TryParse(listAttributes.SelectedValue, out id))
            {
                TransitAttribute attribute = SystemService.GetAttributeById(id);
                inputDefaultUrl.Text = Renderer.Render(attribute.DefaultUrl);
                inputDefaultValue.Text = Renderer.Render(attribute.DefaultValue);
                previewImage.ImageUrl = string.Format("SystemAttribute.aspx?id={0}", attribute.Id);
                previewImage.Visible = true;
            }
            else
            {
                previewImage.Visible = false;
                inputDefaultUrl.Text = string.Empty;
                inputDefaultValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
