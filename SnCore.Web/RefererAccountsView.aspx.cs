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
using SnCore.Services;
using SnCore.WebServices;

public partial class RefererAccountsView : AccountPersonPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                linkAdministrator.OnClientClick =
                    string.Format("location.href='mailto:{0}';",
                        SystemService.GetConfigurationByNameWithDefault(
                            "SnCore.Admin.EmailAddress", "admin@localhost.com").Value);
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
    private void GetData()
    {
        gridManage.CurrentPage = 0;
        gridManage.VirtualItemCount = StatsService.GetRefererAccountsCount();
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageSize = gridManage.PageSize;
            serviceoptions.PageNumber = gridManage.CurrentPage;
            gridManage.DataSource = StatsService.GetRefererAccounts(serviceoptions);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
