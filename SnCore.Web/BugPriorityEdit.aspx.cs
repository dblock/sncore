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

public partial class BugPriorityEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Bugs", Request, "BugProjectsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Priorities", Request, "BugPrioritiesManage.aspx"));

            if (RequestId > 0)
            {
                TransitBugPriority t = SessionManager.BugService.GetBugPriorityById(RequestId);
                inputName.Text = t.Name;
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Priority", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitBugPriority t = new TransitBugPriority();
        t.Name = inputName.Text;
        t.Id = RequestId;
        SessionManager.BugService.CreateOrUpdateBugPriority(SessionManager.Ticket, t);
        Redirect("BugPrioritiesManage.aspx");
    }
}
