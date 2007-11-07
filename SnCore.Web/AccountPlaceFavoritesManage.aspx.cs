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
using SnCore.WebServices;
using SnCore.SiteMap;
using SnCore.Services;

public partial class AccountPlaceFavoritesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        favoritesList.OnGetDataSource += new EventHandler(favoritesList_OnGetDataSource);

        if (!IsPostBack)
        {
            favoritesList.VirtualItemCount = SessionManager.PlaceService.GetAccountPlaceFavoritesCountByAccountId(
                SessionManager.Ticket, SessionManager.AccountId);
            favoritesList_OnGetDataSource(this, null);
            favoritesList.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "AccountPlacesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Favorites", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void favoritesList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = favoritesList.CurrentPageIndex;
        options.PageSize = favoritesList.PageSize;
        favoritesList.DataSource = SessionManager.PlaceService.GetAccountPlaceFavoritesByAccountId(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    private enum Cells
    {
        id = 0
    };

    public void favoritesList_Command(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.CommandArgument.ToString());
                SessionManager.Delete<TransitAccountPlaceFavorite>(id, SessionManager.PlaceService.DeleteAccountPlaceFavorite);
                ReportInfo("Favorite place deleted.");
                favoritesList.CurrentPageIndex = 0;
                favoritesList_OnGetDataSource(source, e);
                favoritesList.DataBind();
                break;
        }
    }
}
