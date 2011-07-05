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
using System.Text;
using SnCore.Services;
using SnCore.WebServices;
using System.Collections.Generic;
using SnCore.SiteMap;

public partial class PlacesFavoritesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
            gridManageFavorites.OnGetDataSource += new EventHandler(gridManageFavorites_OnGetDataSource);

            if (!IsPostBack)
            {
                GetDataFavorites(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Eat Out", Request, "PlacesView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Favorites", Request.Url));
                StackSiteMap(sitemapdata);
            }
    }

    private void GetDataFavorites(object sender, EventArgs e)
    {
        gridManageFavorites.CurrentPageIndex = 0;
        gridManageFavorites.VirtualItemCount = SessionManager.GetCount<TransitPlace>(
            SessionManager.PlaceService.GetFavoritePlacesCount);
        gridManageFavorites_OnGetDataSource(sender, e);
        gridManageFavorites.DataBind();
    }

    void gridManageFavorites_OnGetDataSource(object sender, EventArgs e)
    {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions(
                gridManageFavorites.PageSize, gridManageFavorites.CurrentPageIndex);
            gridManageFavorites.DataSource = SessionManager.GetCollection<TransitPlace>(
                serviceoptions, SessionManager.PlaceService.GetFavoritePlaces);
    }
}
