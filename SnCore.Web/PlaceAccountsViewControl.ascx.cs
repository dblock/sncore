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
using SnCore.WebServices;
using System.Collections.Generic;

public partial class PlaceAccountsViewControl : Control
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
        try
        {
            accountsList.OnGetDataSource += new EventHandler(accountsList_OnGetDataSource);

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
        accountsList.CurrentPageIndex = 0;
        accountsList.VirtualItemCount = PlaceService.GetAccountPlacesCountByPlaceId(PlaceId);
        accountsList_OnGetDataSource(sender, e);
        accountsList.DataBind();
        this.Visible = (accountsList.VirtualItemCount > 0);
    }

    void accountsList_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = accountsList.CurrentPageIndex;
            options.PageSize = accountsList.PageSize;
            accountsList.DataSource = PlaceService.GetAccountPlacesByPlaceId(PlaceId, options);
            panelGrid.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
