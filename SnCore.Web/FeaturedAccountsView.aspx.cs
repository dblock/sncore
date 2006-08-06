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
using System.Reflection;
using System.Collections.Generic;

public partial class FeaturedAccountsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
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
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SystemService.GetFeaturesCount("Account");
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();

        if (gridManage.VirtualItemCount == 0)
        {
            labelCount.Text = "No Featured People";
        }
        else if (gridManage.VirtualItemCount == 1)
        {
            labelCount.Text = "1 Featured Person";
        }
        else
        {
            labelCount.Text = string.Format("{0} Featured People!", gridManage.VirtualItemCount);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageSize = gridManage.PageSize;
            serviceoptions.PageNumber = gridManage.CurrentPageIndex;
            gridManage.DataSource = SystemService.GetFeatures("Account", serviceoptions);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public TransitAccount GetAccount(int id)
    {
        object[] args = { id };
        return SessionManager.GetCachedItem<TransitAccount>(AccountService, "GetAccountById", args);
    }

    public string GetDescription(int id)
    {
        object[] args = { id };
        List<TransitAccountProfile> profiles = SessionManager.GetCachedCollection<TransitAccountProfile>(
            AccountService, "GetAccountProfilesById", args);

        if (profiles == null || profiles.Count == 0)
            return string.Empty;

        return profiles[0].AboutSelf;
    }
}
