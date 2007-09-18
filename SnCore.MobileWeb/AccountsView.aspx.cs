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
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

[SiteMapDataAttribute("People")]
public partial class AccountsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<
            SocialService.TransitAccountActivity, SocialService.ServiceQueryOptions, SocialService.AccountActivityQueryOptions>(
            GetQueryOptions(), SessionManager.SocialService.GetAccountActivityCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private SocialService.AccountActivityQueryOptions GetQueryOptions()
    {
        SocialService.AccountActivityQueryOptions options = new SocialService.AccountActivityQueryOptions();
        return options;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        SocialService.AccountActivityQueryOptions options = GetQueryOptions();

        SocialService.ServiceQueryOptions serviceoptions = new SocialService.ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<
            SocialService.TransitAccountActivity, SocialService.ServiceQueryOptions, SocialService.AccountActivityQueryOptions>(
            options, serviceoptions, SessionManager.SocialService.GetAccountActivity);
    }
}
