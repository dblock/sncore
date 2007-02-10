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

public partial class BugNoteEdit : AuthenticatedPage
{
    public int BugId
    {
        get
        {
            return GetId("bid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitBug bug = SessionManager.BugService.GetBugById(
                SessionManager.Ticket, BugId);
            TransitBugProject project = SessionManager.BugService.GetBugProjectById(
                SessionManager.Ticket, bug.ProjectId);

            imageBugType.ImageUrl = string.Format("images/bugs/type_{0}.gif", bug.Type);
            bugType.Text = Renderer.Render(bug.Type);
            bugId.Text = string.Format("#{0}", bug.Id);
            bugSubject.Text = Renderer.Render(bug.Subject);
            bugType.Text = Renderer.Render(bug.Type);
            linkBack.NavigateUrl = string.Format("BugView.aspx?id={0}", bug.Id);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Bugs", Request, "BugProjectsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(project.Name, Request, string.Format("BugProjectBugsManage.aspx?id={0}", project.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode(string.Format("#{0}: {1}", bug.Id, bug.Subject), Request, string.Format("BugView.aspx?id={0}", bug.Id)));

            if (RequestId > 0)
            {
                TransitBugNote t = SessionManager.BugService.GetBugNoteById(
                    SessionManager.Ticket, RequestId);
                inputNote.Text = t.Details;
                sitemapdata.Add(new SiteMapDataAttributeNode("Edit Note", Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Note", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitBugNote t = new TransitBugNote();
        t.Details = inputNote.Text;
        t.Id = RequestId;
        t.BugId = BugId;
        SessionManager.CreateOrUpdate<TransitBugNote>(
            t, SessionManager.BugService.CreateOrUpdateBugNote);
        Redirect(string.Format("BugView.aspx?id={0}", BugId));
    }
}
