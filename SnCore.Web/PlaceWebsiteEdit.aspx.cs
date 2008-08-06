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
using SnCore.Data.Hibernate;
using System.IO;
using SnCore.Tools.Web.Html;

public partial class PlaceWebsiteEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));

            TransitPlace place = SessionManager.GetInstance<TransitPlace, int>(
                PlaceId, SessionManager.PlaceService.GetPlaceById);
            sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request, string.Format("PlaceView.aspx?id={0}", place.Id)));

            linkBack.NavigateUrl = string.Format("PlaceView.aspx?id={0}", place.Id);

            DomainClass cs = SessionManager.GetDomainClass("PlaceWebsite");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputUrl.MaxLength = cs["Url"].MaxLengthInChars;

            if (RequestId > 0)
            {
                TransitPlaceWebsite tw = SessionManager.GetInstance<TransitPlaceWebsite, int>(
                    RequestId, SessionManager.PlaceService.GetPlaceWebsiteById);
                inputName.Text = tw.Name;
                inputUrl.Text = tw.Url;
                inputDescription.Text = tw.Description;
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("Link a Site", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public int PlaceId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitPlaceWebsite tw = new TransitPlaceWebsite();
        tw.Name = inputName.Text;

        if (string.IsNullOrEmpty(inputUrl.Text) || inputUrl.Text == "http://")
            throw new Exception("Missing url");

        if (!Uri.IsWellFormedUriString(inputUrl.Text, UriKind.Absolute))
            inputUrl.Text = "http://" + inputUrl.Text;

        if (string.IsNullOrEmpty(inputName.Text))
            throw new Exception("Missing title");

        tw.Url = inputUrl.Text;
        tw.Description = inputDescription.Text;
        tw.Id = RequestId;
        tw.PlaceId = PlaceId;

        TransitPlaceWebsite t_existing = SessionManager.GetInstance<TransitPlaceWebsite, int, string>(
            PlaceId, tw.Url, SessionManager.PlaceService.GetPlaceWebsiteByUri);

        if (t_existing != null && t_existing.Id != RequestId)
        {
            throw new Exception(string.Format("The website \"{0}\" has already been added.", tw.Url));
        }

        SessionManager.CreateOrUpdate<TransitPlaceWebsite>(
            tw, SessionManager.PlaceService.CreateOrUpdatePlaceWebsite);

        Redirect(string.Format("PlaceView.aspx?id={0}", tw.PlaceId));
    }

    public void linkFetch_Click(object sender, EventArgs e)
    {
        try
        {
            ContentPageParameters p = new ContentPageParameters();
            p.UserAgent = SessionManager.GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0");
            Uri pageuri = new Uri(inputUrl.Text);
            string content = ContentPage.GetHttpContent(pageuri, p);
            HtmlPageInfo info = HtmlPageInfoExtractor.Extract(content);
            if (string.IsNullOrEmpty(info.Title))
            {
                TransitPlace place = SessionManager.GetInstance<TransitPlace, int>(
                    PlaceId, SessionManager.PlaceService.GetPlaceById);
                info.Title = place.Name;
            }
            inputName.Text = info.Title.Trim();
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("Error retreiving {0}\n{1}",
                inputUrl.Text, ex.Message));
        }
    }
}
