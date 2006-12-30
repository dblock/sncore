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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class PlaceTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Place Types", Request, "PlaceTypesManage.aspx"));

                if (RequestId > 0)
                {
                    TransitPlaceType t = SessionManager.PlaceService.GetPlaceTypeById(RequestId);
                    inputName.Text = t.Name;
                    sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Place Type", Request.Url));
                }
                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
            TransitPlaceType t = new TransitPlaceType();
            t.Name = inputName.Text;
            t.Id = RequestId;
            SessionManager.PlaceService.CreateOrUpdatePlaceType(SessionManager.Ticket, t);
            Redirect("PlaceTypesManage.aspx");
    }
}
