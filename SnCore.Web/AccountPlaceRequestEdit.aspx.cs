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

public partial class AccountPlaceRequestEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            inputType.DataSource = SessionManager.PlaceService.GetAccountPlaceTypes(
                SessionManager.Ticket, null);
            inputType.DataBind();

            if (ParentId != 0)
            {
                TransitPlace place = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, ParentId);
                linkPlace.NavigateUrl = string.Format("PlaceView.aspx?id={0}", place.Id);
                linkBack.NavigateUrl = (string.IsNullOrEmpty(ReturnUrl)) ? linkPlace.NavigateUrl : ReturnUrl;
                linkPlace.Text = Renderer.Render(place.Name);
                imagePlace.ImageUrl = string.Format("PlacePictureThumbnail.aspx?id={0}", place.PictureId);
                inputMessage.Text = "Hello,\n\nI work for this place and would like to manage content.\n\nThanks!\n";

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request, string.Format("PlaceView.aspx?id={0}", place.Id)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Request Ownership", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }

        SetDefaultButton(manageAdd);
    }

    public string ReturnUrl
    {
        get
        {
            string result = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(result)) result = string.Format("PlaceView.aspx?id={0}", ParentId);
            return result;
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountPlaceRequest request = new TransitAccountPlaceRequest();
        request.AccountId = SessionManager.Account.Id;
        request.Message = inputMessage.Text;
        request.PlaceId = ParentId;
        request.Type = inputType.SelectedValue;
        SessionManager.PlaceService.CreateOrUpdateAccountPlaceRequest(SessionManager.Ticket, request);
        panelRequest.Visible = false;
        ReportInfo("Request sent.");
    }

    public int ParentId
    {
        get
        {
            return GetId("pid");
        }
    }
}
