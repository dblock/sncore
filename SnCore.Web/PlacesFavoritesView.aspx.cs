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
        try
        {
            gridManageFavorites.OnGetDataSource += new EventHandler(gridManageFavorites_OnGetDataSource);

            if (!IsPostBack)
            {
                GetDataFavorites(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Favorites", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private void GetDataFavorites(object sender, EventArgs e)
    {
        gridManageFavorites.CurrentPageIndex = 0;
        gridManageFavorites.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            PlaceService, "GetFavoritePlacesCount", null);
        gridManageFavorites_OnGetDataSource(sender, e);
        gridManageFavorites.DataBind();
    }

    void gridManageFavorites_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions(
                gridManageFavorites.PageSize, gridManageFavorites.CurrentPageIndex);
            object[] args = { serviceoptions };
            gridManageFavorites.DataSource = SessionManager.GetCachedCollection<TransitPlace>(
                PlaceService, "GetFavoritePlaces", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
