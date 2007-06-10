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
using System.Drawing;
using SnCore.Tools.Drawing;
using System.IO;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class SystemAttributeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Attributes", Request, "SystemAttributesManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("Attribute");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputDefaultUrl.MaxLength = cs["DefaultUrl"].MaxLengthInChars;

            if (RequestId > 0)
            {
                TransitAttribute t = SessionManager.ObjectService.GetAttributeById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                inputDefaultUrl.Text = t.DefaultUrl;
                inputDefaultValue.Text = t.DefaultValue;
                imageBitmap.ImageUrl = string.Format("SystemAttribute.aspx?id={0}&CacheDuration=0", t.Id);
                imageBitmap.Visible = t.HasBitmap;
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                imageBitmap.Visible = false;
                sitemapdata.Add(new SiteMapDataAttributeNode("New Attribute", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAttribute t = new TransitAttribute();
        t.Name = inputName.Text;
        t.Description = inputDescription.Text;
        t.DefaultUrl = inputDefaultUrl.Text;
        t.DefaultValue = inputDefaultValue.Text;
        t.Id = RequestId;
        if (inputBitmap.HasFile) t.Bitmap = new ThumbnailBitmap(inputBitmap.FileContent, new Size(16, 16)).Bitmap;
        SessionManager.CreateOrUpdate<TransitAttribute>(
            t, SessionManager.ObjectService.CreateOrUpdateAttribute);
        Redirect("SystemAttributesManage.aspx");
    }
}
