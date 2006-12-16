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

public partial class PlacePictureEdit : AuthenticatedPage
{
    public int PlaceId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitPlace p = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, PlaceId);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("PlaceView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request, string.Format("PlacePicturesManage.aspx?id={0}", p.Id)));

            if (RequestId > 0)
            {
                TransitPlacePicture t = SessionManager.PlaceService.GetPlacePictureById(RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                imageFull.ImageUrl = string.Format("PlacePicture.aspx?id={0}&CacheDuration=0", t.Id);
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
        TransitPlacePicture t = new TransitPlacePicture();
        t.Name = inputName.Text;
        t.Description = inputDescription.Text;
        t.PlaceId = PlaceId;
        t.Id = RequestId;
        SessionManager.PlaceService.CreateOrUpdatePlacePicture(SessionManager.Ticket, t);
        Redirect(string.Format("PlacePicturesManage.aspx?id={0}", PlaceId));
    }
}
