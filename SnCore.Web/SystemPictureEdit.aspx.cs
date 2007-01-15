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
using SnCore.SiteMap;

public partial class SystemPictureEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request, "SystemPicturesManage.aspx"));

            inputType.DataSource = SessionManager.ObjectService.GetPictureTypes(
                SessionManager.Ticket, null);

            inputType.DataBind();

            if (RequestId > 0)
            {
                TransitPicture t = SessionManager.ObjectService.GetPictureById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                inputType.Items.FindByValue(t.Type).Selected = true;
                imageThumbnail.ImageUrl = string.Format("SystemPictureThumbnail.aspx?id={0}&CacheDuration=0", t.Id);
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Picture", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitPicture t = new TransitPicture();
        t.Name = inputName.Text;
        t.Description = inputDescription.Text;
        t.Type = inputType.SelectedValue;
        t.Id = RequestId;
        SessionManager.ObjectService.CreateOrUpdatePicture(SessionManager.Ticket, t);
        Redirect("SystemPicturesManage.aspx");
    }
}
