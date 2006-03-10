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
        try
        {
            placesList.OnGetDataSource += new EventHandler(placesList_OnGetDataSource);

            if (!IsPostBack)
            {
                placesList_OnGetDataSource(this, null);
                placesList.DataBind();
                this.Visible = placesList.Items.Count > 0;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void placesList_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            placesList.DataSource = PlaceService.GetAccountPlacesByAccountId(AccountId);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
