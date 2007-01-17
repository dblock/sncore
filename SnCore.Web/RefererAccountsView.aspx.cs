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
using SnCore.SiteMap;

public partial class RefererAccountsView : AccountPersonPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            //labelName.Text = Renderer.Render(SessionManager.GetCachedConfiguration(
            //            "SnCore.Name", "SnCore"));

            //linkAdministrator.OnClientClick =
            //    string.Format("location.href='mailto:{0}';",
            //       SessionManager.GetCachedConfiguration(
            //            "SnCore.Admin.EmailAddress", "admin@localhost.com"));
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Top Traffickers", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }
    private void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitRefererAccount>(
            SessionManager.StatsService.GetRefererAccountsCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
        gridManage.DataSource = SessionManager.GetCollection<TransitRefererAccount>(
            serviceoptions, SessionManager.StatsService.GetRefererAccounts);
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    public string LinkMailToAdministrator
    {
        get
        {
            return string.Format("location.href='mailto:{0}';",
                SessionManager.GetCachedConfiguration(
                    "SnCore.Admin.EmailAddress", "admin@localhost.com"));
        }
    }
}
