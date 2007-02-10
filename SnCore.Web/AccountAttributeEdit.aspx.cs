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
using SnCore.SiteMap;

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
        if (!IsPostBack)
        {
            TransitAccount ta = SessionManager.AccountService.GetAccountById(
                SessionManager.Ticket, AccountId);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(ta.Name, Request, string.Format("AccountView.aspx?id={0}", ta.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Attributes", Request, string.Format("AccountAttributesManage.aspx?id={0}", ta.Id)));

            linkBack.NavigateUrl = string.Format("AccountAttributesManage.aspx?id={0}", AccountId);

            List<TransitAttribute> attributes = SessionManager.ObjectService.GetAttributes(
                SessionManager.Ticket, null);

            if (RequestId == 0)
            {
                attributes.Insert(0, new TransitAttribute());
            }

            listAttributes.DataSource = attributes;
            listAttributes.DataBind();

            if (RequestId > 0)
            {
                TransitAccountAttribute attribute = SessionManager.AccountService.GetAccountAttributeById(
                    SessionManager.Ticket, RequestId);
                inputValue.Text = Renderer.Render(attribute.Value);
                inputUrl.Text = Renderer.Render(attribute.Url);
                inputDefaultUrl.Text = Renderer.Render(attribute.Attribute.DefaultUrl);
                inputDefaultValue.Text = Renderer.Render(attribute.Attribute.DefaultValue);
                listAttributes.Items.FindByValue(attribute.AttributeId.ToString()).Selected = true;
                listAttributes.Enabled = false;
                previewImage.ImageUrl = string.Format("SystemAttribute.aspx?id={0}", attribute.AttributeId);
                previewImage.Visible = true;
                sitemapdata.Add(new SiteMapDataAttributeNode(attribute.Attribute.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Attribute", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountAttribute attribute = new TransitAccountAttribute();

        attribute.Url = inputUrl.Text;
        attribute.Value = inputValue.Text;
        attribute.Id = RequestId;
        attribute.AttributeId = int.Parse(listAttributes.SelectedValue);
        attribute.AccountId = AccountId;
        SessionManager.CreateOrUpdate<TransitAccountAttribute>(
            attribute, SessionManager.AccountService.CreateOrUpdateAccountAttribute);
        Redirect(linkBack.NavigateUrl);

    }

    public void listAttributes_SelectedItemChanged(object sender, EventArgs e)
    {
        int id = 0;
        if (int.TryParse(listAttributes.SelectedValue, out id))
        {
            TransitAttribute attribute = SessionManager.ObjectService.GetAttributeById(
                SessionManager.Ticket, id);
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
}
