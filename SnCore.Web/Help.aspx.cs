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

public partial class Help : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {   
        try
        {
            if (!IsPostBack)
            {
                linkAdministrator.OnClientClick = 
                    string.Format("location.href='mailto:{0}';",
                        SystemService.GetConfigurationByNameWithDefault(
                            "SnCore.Admin.EmailAddress", "admin@localhost.com").Value);
                        
                linkSuggestFeature.NavigateUrl =
                    string.Format("BugEdit.aspx?pid={0}&type=Suggestion",
                        SystemService.GetConfigurationByNameWithDefault(
                            "SnCore.NewFeature.ProjectId", "0").Value);

                linkReportBug.NavigateUrl =
                    string.Format("BugEdit.aspx?pid={0}&type=Bug",
                        SystemService.GetConfigurationByNameWithDefault(
                            "SnCore.Bug.ProjectId", "0").Value);

                linkSiteDiscussion.NavigateUrl=
                    string.Format("DiscussionView.aspx?id={0}",
                        SystemService.GetConfigurationByNameWithDefault(
                            "SnCore.Site.DiscussionId", "0").Value);

            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
