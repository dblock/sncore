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
using SnCore.SiteMap;

public partial class AccountSurveysManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Surveys", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.ObjectService.GetSurveys(
            SessionManager.Ticket, null);
    }
}
