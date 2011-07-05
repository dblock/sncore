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
using SnCore.SiteMap;

public partial class PlaceView : Page
{
    private int mPlaceId = -1;
    private PlaceService.TransitPlace mPlace = null;

    public int PlaceId
    {
        get
        {
            if (mPlaceId < 0)
            {
                mPlaceId = RequestId;
            }

            return mPlaceId;
        }
    }

    public PlaceService.TransitPlace Place
    {
        get
        {
            if (mPlace == null)
            {
                mPlace = SessionManager.GetInstance<PlaceService.TransitPlace, PlaceService.ServiceQueryOptions, int>(
                    PlaceId, SessionManager.PlaceService.GetPlaceById);
            }
            return mPlace;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Place == null)
            {
                throw new Exception("Place does not exist.");
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Eat Out", Request, "PlacesView.aspx"));
            sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "PlacesView.aspx",
                Place.Country, Place.State, Place.City, Place.Neighborhood, Place.Type));
            sitemapdata.Add(new SiteMapDataAttributeNode(Place.Name, Request.Url));
            StackSiteMap(sitemapdata);

            this.Title = Renderer.Render(string.Format("{0}, {1}", Place.Name, Place.City));

            placeZip.Text = Renderer.Render(Place.Zip);
            placeStreet.Text = Renderer.Render(Place.Street);
            placeCrossStreet.Text = Renderer.Render(Place.CrossStreet);
            placeType.Text = Renderer.Render(Place.Type);
            placeCity.Text = Renderer.Render(Place.City);
            placeState.Text = Renderer.Render(Place.State);
            placeCountry.Text = Renderer.Render(Place.Country);

            placeImage.Visible = Place.PictureId != 0;
            placeImage.ImageUrl = string.Format("PlacePictureThumbnail.aspx?id={0}",
                Place.PictureId);

            linkPictures.NavigateUrl = placeLinkPictures.HRef = string.Format("PlacePicturesView.aspx?id={0}",
                Place.Id);

            GetReviews(sender, e);
        }
    }

    private void GetReviews(object sender, EventArgs e)
    {
        int reviews_id = SessionManager.GetCount<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions, string, int>(
            "Place", Place.Id, SessionManager.DiscussionService.GetOrCreateDiscussionId);

        linkReviews.NavigateUrl = string.Format("DiscussionView.aspx?id={0}", reviews_id);
    }
}
