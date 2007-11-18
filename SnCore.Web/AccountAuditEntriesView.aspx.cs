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

public partial class AccountAuditEntriesView : AuthenticatedPage
{
    void gridActivity_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridActivity.PageSize;
        options.PageNumber = gridActivity.CurrentPageIndex;
        gridActivity.DataSource = SessionManager.GetCollection<TransitAccountAuditEntry>(
            options, SessionManager.SocialService.GetAccountAuditEntries);
    }

    private void GetData()
    {
        gridActivity.CurrentPageIndex = 0;
        gridActivity.VirtualItemCount = SessionManager.GetCount<TransitAccountAuditEntry>(
            SessionManager.SocialService.GetAccountAuditEntriesCount);
        gridActivity_OnGetDataSource(this, null);
        gridActivity.DataBind();
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridActivity.OnGetDataSource += new EventHandler(gridActivity_OnGetDataSource);

        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("User Activity", Request.Url));
            StackSiteMap(sitemapdata);

            GetData();
        }
    }
}
