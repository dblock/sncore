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
                TransitPlacePicture p = PlaceService.GetPlacePictureById(RequestId);
                inputPicture.Src = string.Format("PlacePicture.aspx?id={0}", RequestId);
                inputName.Text = Renderer.Render(p.Name);
                inputDescription.Text = Renderer.Render(p.Description);
                inputCreated.Text = Adjust(p.Created).ToString();

                TransitPlace l = PlaceService.GetPlaceById(p.PlaceId);
                this.Title = string.Format("{0} {1}", Renderer.Render(l.Name), Renderer.Render(p.Name));

                labelPicture.Text = Renderer.Render(p.Name);
                
                linkPlace.Text = Renderer.Render(l.Name);
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

                linkPlace.NavigateUrl = string.Format("PlaceView.aspx?id={0}", l.Id);

                linkComments.Visible = p.CommentCount > 0;
                linkComments.Text = string.Format("{0} comment{1}",
                    (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
                    (p.CommentCount == 1) ? "" : "s");

                discussionComments.DiscussionId = DiscussionService.GetPlacePictureDiscussionId(RequestId);
                discussionComments.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
