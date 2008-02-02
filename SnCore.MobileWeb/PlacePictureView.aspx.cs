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
using PlaceService;

public partial class PlacePictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PlaceService.TransitPlacePicture p = GetPlacePicture();
            PlaceService.TransitPlace a = GetPlace();

            GetPictureData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(a.Name, Request, string.Format("PlaceView.aspx?id={0}", a.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request, string.Format("PlacePicturesView.aspx?id={0}", a.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }

    }

    PlaceService.TransitPlacePicture GetPlacePicture()
    {
        return SessionManager.GetInstance<TransitPlacePicture, PlaceService.ServiceQueryOptions, int>(
                            RequestId, SessionManager.PlaceService.GetPlacePictureById);
    }

    public PlaceService.TransitPlace GetPlace()
    {
        return SessionManager.GetInstance<TransitPlace, PlaceService.ServiceQueryOptions, int>(
            GetPlacePicture().PlaceId, SessionManager.PlaceService.GetPlaceById);
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitPlacePicture p = GetPlacePicture();

        inputPicture.Src = string.Format("PlacePictureThumbnail.aspx?id={0}",
            p.Id);

        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = SessionManager.Adjust(p.Created).ToString("d");
        inputCounter.Text = p.Counter.Total.ToString();

        TransitPlace l = GetPlace();

        //discussionComments.ReturnUrl = string.Format("PlacePictureView.aspx?id={0}", PictureId);
        //discussionComments.DiscussionId = SessionManager.GetCount<TransitDiscussion, string, int>(
        //    typeof(PlacePicture).Name, PictureId, SessionManager.DiscussionService.GetOrCreateDiscussionId);
        //discussionComments.DataBind();
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
