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

public partial class PlacePictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);
            if (!IsPostBack)
            {
                mPictureId = RequestId;
                GetPictureData(sender, e);
                GetPicturesData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
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
                object[] sp_args = { PictureId };
                mPlacePicture = SessionManager.GetCachedItem<TransitPlacePicture>(
                    PlaceService, "GetPlacePictureById", sp_args);
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
                object[] as_args = { PlacePicture.PlaceId };
                mPlace = SessionManager.GetCachedItem<TransitPlace>(
                    PlaceService, "GetPlaceById", as_args);
            }
            return mPlace;
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        object[] p_args = { Place.Id };
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            PlaceService, "GetPlacePicturesCountById", p_args);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitPlacePicture p = PlacePicture;

        inputPicture.Src = string.Format("PlacePicture.aspx?id={0}", p.Id);
        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = Adjust(p.Created).ToString();

        TransitPlace l = Place;

        this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        labelPicture.Text = string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name);
        labelPlaceName.Text = linkPlace.Text = Renderer.Render(l.Name);
        linkCity.Text = Renderer.Render(l.City);
        linkState.Text = Renderer.Render(l.State);
        linkCountry.Text = Renderer.Render(l.Country);
        linkType.Text = l.Type + "s";

        linkType.NavigateUrl = string.Format("PlacesView.aspx?city={0}&state={1}&country={2}&type={3}",
            Renderer.UrlEncode(l.City),
            Renderer.UrlEncode(l.State),
            Renderer.UrlEncode(l.Country),
            Renderer.UrlEncode(l.Type));

        linkCity.NavigateUrl = string.Format("PlacesView.aspx?city={0}&state={1}&country={2}",
            Renderer.UrlEncode(l.City),
            Renderer.UrlEncode(l.State),
            Renderer.UrlEncode(l.Country));

        linkState.NavigateUrl = string.Format("PlacesView.aspx?state={0}&country={1}",
            Renderer.UrlEncode(l.State),
            Renderer.UrlEncode(l.Country));

        linkCountry.NavigateUrl = string.Format("PlacesView.aspx?country={0}",
            Renderer.UrlEncode(l.Country));

        linkBack.NavigateUrl = linkPlace.NavigateUrl = string.Format("PlaceView.aspx?id={0}", l.Id);
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

        discussionComments.DiscussionId = DiscussionService.GetPlacePictureDiscussionId(PictureId);
        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, CommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Picture":
                    mPictureId = int.Parse(e.CommandArgument.ToString());
                    GetPictureData(source, e);
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
            object[] args = { Place.Id, options };
            picturesView.DataSource = SessionManager.GetCachedCollection<TransitPlacePicture>(
                PlaceService, "GetPlacePicturesById", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
