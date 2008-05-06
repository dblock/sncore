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
using SnCore.Services;

public partial class PlaceAccountEventsViewControl : Control
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
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        TransitAccountEventInstanceQueryOptions options = GetQueryOptions();
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountEventInstance, TransitAccountEventInstanceQueryOptions>(
            options, SessionManager.EventService.GetAccountEventInstancesCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
        this.Visible = (gridManage.VirtualItemCount > 0);
    }

    private TransitAccountEventInstanceQueryOptions GetQueryOptions()
    {
        TransitAccountEventInstanceQueryOptions options = new TransitAccountEventInstanceQueryOptions();
        options.PlaceId = PlaceId;
        options.StartDateTime = base.Adjust(DateTime.UtcNow).Date;
        options.EndDateTime = DateTime.MaxValue;
        return options;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        TransitAccountEventInstanceQueryOptions options = GetQueryOptions();
        ServiceQueryOptions service_options = new ServiceQueryOptions();
        service_options.PageNumber = gridManage.CurrentPageIndex;
        service_options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountEventInstance, TransitAccountEventInstanceQueryOptions>(
            options, service_options, SessionManager.EventService.GetAccountEventInstances);
    }
}
