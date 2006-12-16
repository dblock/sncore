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

[SiteMapDataAttribute("Help")]
public partial class Help : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkAdministrator.OnClientClick =
                string.Format("location.href='mailto:{0}';",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Admin.EmailAddress", "admin@localhost.com"));

            linkSuggestFeature.NavigateUrl =
                string.Format("BugEdit.aspx?pid={0}&type=Suggestion",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.NewFeature.ProjectId", "0"));

            linkReportBug.NavigateUrl =
                string.Format("BugEdit.aspx?pid={0}&type=Bug",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Bug.ProjectId", "0"));

            linkSiteDiscussion.NavigateUrl =
                string.Format("DiscussionView.aspx?id={0}",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Site.DiscussionId", "0"));

        }
    }
}
