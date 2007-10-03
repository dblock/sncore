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
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class SystemAccountPropertyEdit : AuthenticatedPage
{
    private TransitAccountPropertyGroup mPropertyGroup = null;

    public int PropertyGroupId
    {
        get
        {
            return GetId("pid");
        }
    }

    public TransitAccountPropertyGroup PropertyGroup
    {
        get
        {
            if (mPropertyGroup == null)
            {
                mPropertyGroup = SessionManager.AccountService.GetAccountPropertyGroupById(
                    SessionManager.Ticket, PropertyGroupId);
            }

            return mPropertyGroup;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Account Property Groups", Request, "SystemAccountPropertyGroupsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(PropertyGroup.Name, Request, string.Format("SystemAccountPropertyGroupEdit.aspx?id={0}", PropertyGroupId)));

            DomainClass cs = SessionManager.GetDomainClass("AccountProperty");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

            linkBack.NavigateUrl = string.Format("SystemAccountPropertyGroupEdit.aspx?id={0}", PropertyGroupId);

            inputTypeName.Items.Add(new ListItem("String", Type.GetType("System.String").ToString()));
            inputTypeName.Items.Add(new ListItem("Array", Type.GetType("System.Array").ToString()));
            inputTypeName.Items.Add(new ListItem("Text", Type.GetType("System.Text.StringBuilder").ToString()));
            inputTypeName.Items.Add(new ListItem("Integer", Type.GetType("System.Int32").ToString()));
            inputTypeName.Items.Add(new ListItem("Boolean", Type.GetType("System.Boolean").ToString()));
            // inputTypeName.Items.Add(new ListItem(Type.GetType("System.DateTime").ToString()));

            if (RequestId > 0)
            {
                TransitAccountProperty t = SessionManager.AccountService.GetAccountPropertyById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                inputDefaultValue.Text = t.DefaultValue;
                inputPublish.Checked = t.Publish;

                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));

                inputTypeName.ClearSelection();
                ListItem typeitem = inputTypeName.Items.FindByValue(t.Type.ToString());

                if (typeitem == null)
                {
                    typeitem = new ListItem(t.Type.ToString());
                    inputTypeName.Items.Add(typeitem);
                }

                typeitem.Selected = true;
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Property", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountProperty t = new TransitAccountProperty();
        t.Name = inputName.Text;
        t.Type = inputTypeName.SelectedValue;
        t.Description = inputDescription.Text;
        t.DefaultValue = inputDefaultValue.Text;
        t.AccountPropertyGroupId = PropertyGroupId;
        t.Publish = inputPublish.Checked;
        t.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountProperty>(
            t, SessionManager.AccountService.CreateOrUpdateAccountProperty);
        Redirect(linkBack.NavigateUrl);
    }
}
