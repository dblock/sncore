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
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                selectPriority.DataSource = BugService.GetBugPriorities();
                selectPriority.DataBind();
                selectSeverity.DataSource = BugService.GetBugSeverities();
                selectSeverity.DataBind();
                selectType.DataSource = BugService.GetBugTypes();
                selectType.DataBind();


                TransitBugProject project = BugService.GetBugProjectById(ProjectId);
                linkProject.Text = Renderer.Render(project.Name);
                linkProject.NavigateUrl = string.Format("BugProjectBugsManage.aspx?id={0}", project.Id);

                if (RequestId > 0)
                {
                    TransitBug bug = BugService.GetBugById(RequestId);
                    inputSubject.Text = bug.Subject;
                    inputDetails.Text = bug.Details;
                    selectPriority.Items.FindByValue(bug.Priority).Selected = true;
                    selectSeverity.Items.FindByValue(bug.Severity).Selected = true;
                    selectType.Items.FindByValue(bug.Type).Selected = true;
                    linkBack.NavigateUrl = string.Format("BugView.aspx?id={0}", bug.Id);
                    linkBugId.Text = "#" + bug.Id.ToString();
                }
                else
                {
                    string type = Request.QueryString["type"];
                    if (type != null) selectType.Items.FindByValue(type).Selected = true;
                    linkBugId.Text = "New Bug";
                    linkBack.NavigateUrl = string.Format("BugProjectBugsManage.aspx?id={0}", ProjectId);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitBug t = new TransitBug();
            t.Subject = inputSubject.Text;
            t.Details = inputDetails.Text;
            t.Priority = selectPriority.SelectedValue;
            t.Severity = selectSeverity.SelectedValue;
            t.Type = selectType.SelectedValue;
            t.ProjectId = ProjectId;
            t.Id = RequestId;
            int bugid = BugService.CreateOrUpdateBug(SessionManager.Ticket, t);
            Redirect(string.Format("BugView.aspx?id={0}", bugid));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
