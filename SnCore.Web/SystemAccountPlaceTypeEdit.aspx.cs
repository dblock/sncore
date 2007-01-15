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

public partial class SystemAccountPlaceTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Account Place Types", Request, "SystemAccountPlaceTypesManage.aspx"));

            if (RequestId > 0)
            {
                TransitAccountPlaceType t = SessionManager.PlaceService.GetAccountPlaceTypeById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                inputCanWrite.Checked = t.CanWrite;

                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Type", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountPlaceType t = new TransitAccountPlaceType();
        t.Name = inputName.Text;
        t.Description = inputDescription.Text;
        t.CanWrite = inputCanWrite.Checked;
        t.Id = RequestId;
        SessionManager.PlaceService.CreateOrUpdateAccountPlaceType(SessionManager.Ticket, t);
        Redirect("SystemAccountPlaceTypesManage.aspx");
    }
}
