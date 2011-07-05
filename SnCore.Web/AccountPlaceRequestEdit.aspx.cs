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
using System.Collections.Generic;
using SnCore.WebControls;

public partial class AccountPlaceRequestEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData(sender, e);

            if (ParentId != 0)
            {
                TransitPlace place = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, ParentId);
                linkPlace.NavigateUrl = string.Format("PlaceView.aspx?id={0}", place.Id);
                linkBack.NavigateUrl = (string.IsNullOrEmpty(ReturnUrl)) ? linkPlace.NavigateUrl : ReturnUrl;
                linkPlace.Text = Renderer.Render(place.Name);
                imagePlace.ImageUrl = string.Format("PlacePictureThumbnail.aspx?id={0}", place.PictureId);
                inputMessage.Text = "Hello,\n\nI work for this place and would like to manage content.\n\nThanks!\n";

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Eat Out", Request, "PlacesView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request, string.Format("PlaceView.aspx?id={0}", place.Id)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Request Ownership", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }

        SetDefaultButton(manageAdd);
    }

    public void GetData(object sender, EventArgs e)
    {
        IList<TransitAccountPlaceType> types = SessionManager.GetCollection<TransitAccountPlaceType>(
            null, SessionManager.PlaceService.GetAccountPlaceTypes);

        TransitAccountPlaceType selected = null;
        foreach (TransitAccountPlaceType Placetype in types)
        {
            if (Placetype.DefaultType)
            {
                selected = Placetype;
                break;
            }
        }

        if (selected == null)
        {
            types.Insert(0, new TransitAccountPlaceType());
        }

        inputType.DataSource = types;
        inputType.DataBind();

        if (selected != null)
        {
            ListItemManager.TrySelect(inputType, selected.Name);
        }
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
        SessionManager.CreateOrUpdate<TransitAccountPlaceRequest>(
            request, SessionManager.PlaceService.CreateOrUpdateAccountPlaceRequest);
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
