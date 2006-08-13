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

public partial class AccountsByPropertyValueView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

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

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        object[] args = { Request["GroupName"], Request["PropertyName"], Request["PropertyValue"] };
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(AccountService, "GetAccountsByPropertyValueCount", args);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
            object[] args = { Request["GroupName"], Request["PropertyName"], Request["PropertyValue"], serviceoptions };
            gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccount>(AccountService, "GetAccountsByPropertyValue", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
