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
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class BugProjectEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Bugs", Request, "BugProjectsManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("BugProject");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

            if (RequestId > 0)
            {
                TransitBugProject t = SessionManager.BugService.GetBugProjectById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Project", Request.Url));
            }

            StackSiteMap(sitemapdata);
            SetDefaultButton(manageAdd);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitBugProject t = new TransitBugProject();
        t.Name = inputName.Text;
        t.Description = inputDescription.Text;
        t.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitBugProject>(
            t, SessionManager.BugService.CreateOrUpdateBugProject);
        Redirect("BugProjectsManage.aspx");
    }
}
