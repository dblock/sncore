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
using Wilco.Web.UI;
using System.Collections.Generic;
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountPlaceFavoritesViewControl : Control
{
    public int AccountId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountId", 0);
        }
        set
        {
            ViewState["AccountId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            placesList.OnGetDataSource += new EventHandler(placesList_OnGetDataSource);

            if (!IsPostBack)
            {
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        placesList.CurrentPageIndex = 0;
        object[] args = { AccountId };
        placesList.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            PlaceService, "GetAccountPlaceFavoritesCountById", args);
        placesList_OnGetDataSource(sender, e);
        placesList.DataBind();
        this.Visible = (placesList.VirtualItemCount > 0);
    }

    void placesList_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = placesList.CurrentPageIndex;
            options.PageSize = placesList.PageSize;
            object[] args = { AccountId, options };
            placesList.DataSource = SessionManager.GetCachedCollection<TransitAccountPlaceFavorite>(
                PlaceService, "GetAccountPlaceFavoritesByAccountId", args);
            panelGrid.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
