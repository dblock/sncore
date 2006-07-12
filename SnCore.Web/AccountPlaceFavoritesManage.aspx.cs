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

public partial class AccountPlaceFavoritesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            favoritesList.OnGetDataSource += new EventHandler(favoritesList_OnGetDataSource);

            if (!IsPostBack)
            {
                favoritesList.VirtualItemCount = PlaceService.GetAccountPlaceFavoritesCount(SessionManager.Ticket);
                favoritesList_OnGetDataSource(this, null);
                favoritesList.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void favoritesList_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = favoritesList.CurrentPageIndex;
            options.PageSize = favoritesList.PageSize;
            favoritesList.DataSource = PlaceService.GetAccountPlaceFavorites(SessionManager.Ticket, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    };

    public void favoritesList_Command(object source, DataListCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    PlaceService.DeleteAccountPlaceFavorite(SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                    ReportInfo("Favorite place deleted.");
                    favoritesList.CurrentPageIndex = 0;
                    favoritesList_OnGetDataSource(source, e);
                    favoritesList.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
