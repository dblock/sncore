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
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountPlacesViewControl : Control
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
        placesList.OnGetDataSource += new EventHandler(placesList_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        placesList.CurrentPageIndex = 0;
        placesList.VirtualItemCount = SessionManager.GetCount<TransitAccountPlace, int>(
            AccountId, SessionManager.PlaceService.GetAccountPlacesCount);
        placesList_OnGetDataSource(sender, e);
        placesList.DataBind();
        this.Visible = (placesList.VirtualItemCount > 0);
    }

    void placesList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(placesList.PageSize, placesList.CurrentPageIndex);
        placesList.DataSource = SessionManager.GetCollection<TransitAccountPlace, int>(
            AccountId, options, SessionManager.PlaceService.GetAccountPlaces);
        panelGrid.Update();
    }


}
