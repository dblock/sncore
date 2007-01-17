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

public partial class PlacePictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);

        if (!IsPostBack)
        {
            mPictureId = RequestId;
            GetPictureData(sender, e);
            GetPicturesData(sender, e);

            TransitPlace p = Place;
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "PlacesView.aspx", p.Country, p.State, p.City, p.Neighborhood, p.Type));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("PlaceView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }

    }

    private int mPictureId = 0;

    public int PictureId
    {
        get
        {
            if (mPictureId == 0)
            {
                mPictureId = RequestId;
            }
            return mPictureId;
        }
    }

    private TransitPlacePicture mPlacePicture = null;

    TransitPlacePicture PlacePicture
    {
        get
        {
            if (mPlacePicture == null)
            {
                mPlacePicture = SessionManager.GetInstance<TransitPlacePicture, int>(
                    PictureId, SessionManager.PlaceService.GetPlacePictureById);
            }
            return mPlacePicture;
        }
    }

    private TransitPlace mPlace = null;

    public TransitPlace Place
    {
        get
        {
            if (mPlace == null)
            {
                mPlace = SessionManager.GetInstance<TransitPlace, int>(
                    PlacePicture.PlaceId, SessionManager.PlaceService.GetPlaceById);
            }
            return mPlace;
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCount<TransitPlacePicture, int>(
            Place.Id, SessionManager.PlaceService.GetPlacePicturesCount);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitPlacePicture p = PlacePicture;

        inputPicture.Src = string.Format("PlacePicture.aspx?id={0}", p.Id);
        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputUploadedBy.NavigateUrl = string.Format("AccountView.aspx?id={0}", p.AccountId);
        inputUploadedBy.Text = Renderer.Render(p.AccountName);
        inputCreated.Text = Adjust(p.Created).ToString("d");
        inputCounter.Text = p.Counter.Total.ToString();

        TransitPlace l = Place;

        this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        labelPlaceName.Text = Renderer.Render(l.Name);

        linkBack.NavigateUrl = string.Format("PlaceView.aspx?id={0}", l.Id);
        linkBack.Text = string.Format("&#187; Back to {0}", Renderer.Render(l.Name));
        linkComments.Visible = p.CommentCount > 0;
        linkComments.Text = string.Format("&#187; {0} comment{1}",
            (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
            (p.CommentCount == 1) ? "" : "s");

        linkPrev.Enabled = p.PrevId > 0;
        linkPrev.CommandArgument = p.PrevId.ToString();
        linkNext.Enabled = p.NextId > 0;
        linkNext.CommandArgument = p.NextId.ToString();
        labelIndex.Text = string.Format("{0} / {1}", p.Index + 1, p.Count);

        discussionComments.ReturnUrl = string.Format("PlacePictureView.aspx?id={0}", PictureId);
        discussionComments.DiscussionId = SessionManager.DiscussionService.GetOrCreatePlacePictureDiscussionId(
            SessionManager.Ticket, PictureId);
        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Picture":
                mPictureId = int.Parse(e.CommandArgument.ToString());
                GetPictureData(source, e);
                break;
        }
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
        picturesView.DataSource = SessionManager.GetCollection<TransitPlacePicture, int>(
            Place.Id, options, SessionManager.PlaceService.GetPlacePictures);
    }

}
