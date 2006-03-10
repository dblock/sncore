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
                accountsList_OnGetDataSource(this, null);
                accountsList.DataBind();
                this.Visible = accountsList.Items.Count > 0;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void accountsList_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            accountsList.DataSource = PlaceService.GetAccountPlacesByPlaceId(PlaceId);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
