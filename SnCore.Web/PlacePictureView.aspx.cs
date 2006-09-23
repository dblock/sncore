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
    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                GetPictureData(RequestId);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }

    void GetPictureData(int id)
    {
        object[] e_args = { id };
        TransitPlacePicture p = SessionManager.GetCachedItem<TransitPlacePicture>(
            PlaceService, "GetPlacePictureById", e_args);

        inputPicture.Src = string.Format("PlacePicture.aspx?id={0}", id);
        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = Adjust(p.Created).ToString();

        object[] ae_args = { p.PlaceId };
        TransitPlace l = SessionManager.GetCachedItem<TransitPlace>(
            PlaceService, "GetPlaceById", ae_args);

        labelPicture.Text = this.Title = string.Format("{0}: {1}", 
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

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

        if (!IsPostBack)
        {
            object[] p_args = { l.Id };
            picturesView.DataSource = SessionManager.GetCachedCollection<TransitPlacePicture>(
                PlaceService, "GetPlacePictures", p_args);
            picturesView.DataBind();
        }

        object[] d_args = { id };
        discussionComments.DiscussionId = SessionManager.GetCachedCollectionCount(
            DiscussionService, "GetPlacePictureDiscussionId", d_args);

        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Picture":
                    GetPictureData(int.Parse(e.CommandArgument.ToString()));
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
