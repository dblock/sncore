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
using SnCore.BackEndServices;
using SnCore.Services;

public partial class FeaturedPlacesRss : Page
{
    public string Name
    {
        get
        {
            return Renderer.Render(string.Format("{0} Featured Places",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions queryoptions = new ServiceQueryOptions();
            queryoptions.PageNumber = 0;
            queryoptions.PageSize = 10;

            rssRepeater.DataSource = SessionManager.GetCollection<TransitFeature, string>(
                "Place", queryoptions, SessionManager.ObjectService.GetFeatures);

            rssRepeater.DataBind();
        }
    }

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }

    public string Link
    {
        get
        {
            return new Uri(SessionManager.WebsiteUri, "FeaturedPlacesView.aspx").ToString();
        }
    }

    public TransitPlace GetPlace(int id)
    {
        return SessionManager.GetInstance<TransitPlace, int>(
            id, SessionManager.PlaceService.GetPlaceById);
    }
}
