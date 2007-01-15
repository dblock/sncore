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

public partial class BugEdit : AuthenticatedPage
{
    public int ProjectId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            selectPriority.DataSource = SessionManager.BugService.GetBugPriorities(
                SessionManager.Ticket, null);
            selectPriority.DataBind();
            selectSeverity.DataSource = SessionManager.BugService.GetBugSeverities(
                SessionManager.Ticket, null);
            selectSeverity.DataBind();
            selectType.DataSource = SessionManager.BugService.GetBugTypes(
                SessionManager.Ticket, null);
            selectType.DataBind();

            TransitBugProject project = SessionManager.BugService.GetBugProjectById(
                SessionManager.Ticket, ProjectId);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Bugs", Request, "BugProjectsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(project.Name, Request, string.Format("BugProjectBugsManage.aspx?id={0}", project.Id)));

            if (RequestId > 0)
            {
                TransitBug bug = SessionManager.BugService.GetBugById(
                    SessionManager.Ticket, RequestId);
                inputSubject.Text = bug.Subject;
                inputDetails.Text = bug.Details;
                selectPriority.Items.FindByValue(bug.Priority).Selected = true;
                selectSeverity.Items.FindByValue(bug.Severity).Selected = true;
                selectType.Items.FindByValue(bug.Type).Selected = true;
                linkBack.NavigateUrl = string.Format("BugView.aspx?id={0}", bug.Id);

                sitemapdata.Add(new SiteMapDataAttributeNode(string.Format("#{0}: {1}", bug.Id, bug.Subject), Request.Url));
            }
            else
            {
                string type = Request.QueryString["type"];
                if (type != null) selectType.Items.FindByValue(type).Selected = true;
                linkBack.NavigateUrl = string.Format("BugProjectBugsManage.aspx?id={0}", ProjectId);

                if (Request.QueryString["url"] != null)
                    inputSubject.Text = Request.QueryString["url"];

                if (Request.QueryString["message"] != null)
                    inputDetails.Text = Request.QueryString["message"];

                sitemapdata.Add(new SiteMapDataAttributeNode("New Bug", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitBug t = new TransitBug();
        t.Subject = inputSubject.Text;
        t.Details = inputDetails.Text;
        t.Priority = selectPriority.SelectedValue;
        t.Severity = selectSeverity.SelectedValue;
        t.Type = selectType.SelectedValue;
        t.ProjectId = ProjectId;
        t.Id = RequestId;
        int bugid = SessionManager.BugService.CreateOrUpdateBug(SessionManager.Ticket, t);
        Redirect(string.Format("BugView.aspx?id={0}", bugid));
    }
}
