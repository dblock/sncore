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
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {

                TransitBug bug = BugService.GetBugById(BugId);
                TransitBugProject project = BugService.GetBugProjectById(bug.ProjectId);

                linkBugId.Text = "#" + bug.Id.ToString();
                linkProject.Text = Renderer.Render(project.Name);
                linkProject.NavigateUrl = string.Format("BugProjectBugsManage.aspx?id={0}", project.Id);
                imageBugType.ImageUrl = string.Format("images/bugs/type_{0}.gif", bug.Type);
                bugType.Text = Renderer.Render(bug.Type);
                bugId.Text = string.Format("#{0}", bug.Id);
                bugSubject.Text = Renderer.Render(bug.Subject);
                bugType.Text = Renderer.Render(bug.Type);
                linkBack.NavigateUrl = string.Format("BugView.aspx?id={0}", bug.Id);

                if (RequestId > 0)
                {
                    TransitBugNote t = BugService.GetBugNoteById(RequestId);
                    inputNote.Text = t.Details;
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
            TransitBugNote t = new TransitBugNote();
            t.Details = inputNote.Text;
            t.Id = RequestId;
            t.BugId = BugId;
            BugService.CreateOrUpdateBugNote(SessionManager.Ticket, t);
            Redirect(string.Format("BugView.aspx?id={0}", BugId));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
