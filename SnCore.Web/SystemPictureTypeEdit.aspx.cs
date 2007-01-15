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

public partial class SystemPictureTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Picture Types", Request, "SystemPictureTypesManage.aspx"));

                if (RequestId > 0)
                {
                    TransitPictureType t = SessionManager.ObjectService.GetPictureTypeById(
                        SessionManager.Ticket, RequestId);
                    inputName.Text = t.Name;
                    sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Picture Type", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
            TransitPictureType t = new TransitPictureType();
            t.Name = inputName.Text;
            t.Id = RequestId;
            SessionManager.ObjectService.CreateOrUpdatePictureType(SessionManager.Ticket, t);
            Redirect("SystemPictureTypesManage.aspx");
    }
}
