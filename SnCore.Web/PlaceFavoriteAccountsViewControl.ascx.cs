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
using System.Collections.Generic;
using Wilco.Web.UI;
using SnCore.Services;
using SnCore.WebServices;

public partial class PlaceFavoriteAccountsViewControl : Control
{
    public int PlaceId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "PlaceId", 0);
        }
        set
        {
            ViewState["PlaceId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        accountsList.OnGetDataSource += new EventHandler(accountsList_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        accountsList.CurrentPageIndex = 0;
        accountsList.VirtualItemCount = SessionManager.GetCount<TransitAccountPlaceFavorite, int>(
            PlaceId, SessionManager.PlaceService.GetAccountPlaceFavoritesCount);
        accountsList_OnGetDataSource(sender, e);
        accountsList.DataBind();
        this.Visible = (accountsList.VirtualItemCount > 0);
    }

    void accountsList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = accountsList.CurrentPageIndex;
        options.PageSize = accountsList.PageSize;
        accountsList.DataSource = SessionManager.GetCollection<TransitAccountPlaceFavorite, int>(
            PlaceId, options, SessionManager.PlaceService.GetAccountPlaceFavorites);
    }
}
