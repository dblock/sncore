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

public partial class BugView : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridNotes.OnGetDataSource += new EventHandler(gridNotes_OnGetDataSource);

            if (!IsPostBack)
            {
                GetBug();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void GetBug()
    {
        TransitBug bug = BugService.GetBugById(RequestId);
        TransitBugProject project = BugService.GetBugProjectById(bug.ProjectId);

        this.Title = string.Format("{0} #{1}: {2}", Renderer.Render(project.Name), bug.Id, Renderer.Render(bug.Subject));

        linkBugId.Text = "#" + bug.Id.ToString();
        bugProject.Text = linkProject.Text = linkProject2.Text = Renderer.Render(project.Name);
        linkProject.NavigateUrl = linkProject2.NavigateUrl = string.Format("BugProjectBugsManage.aspx?id={0}", project.Id);
        imageBugType.ImageUrl = string.Format("images/bugs/type_{0}.gif", bug.Type);
        bugType.Text = Renderer.Render(bug.Type);
        bugCreated.Text = bug.Created.ToString();
        bugUpdated.Text = bug.Updated.ToString();
        bugDetails.Text = RenderEx(bug.Details);
        bugId.Text = string.Format("#{0}", bug.Id);
        bugPriority.Text = Renderer.Render(bug.Priority);
        bugResolution.Text = Renderer.Render(bug.Resolution);
        bugSeverity.Text = Renderer.Render(bug.Severity);
        bugStatus.Text = Renderer.Render(bug.Status);
        bugSubject.Text = Renderer.Render(bug.Subject);
        bugType.Text = Renderer.Render(bug.Type);
        linkAccount.Text = Renderer.Render(bug.AccountName);
        linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", bug.AccountId);
        linkAddNew.NavigateUrl = string.Format("BugEdit.aspx?pid={0}", bug.ProjectId);

        switch (bug.Status)
        {
            case "Closed":
                linkAction.Text = "Reopen Bug";
                linkAction.CommandName = "Reopen";
                break;
            case "Resolved":
                linkAction.Text = "Close Bug";
                linkAction.CommandName = "Close";
                break;
            default:
                linkAction.Text = "Resolve Bug";
                linkAction.CommandName = "Resolve";
                break;
        }

        linkEdit.NavigateUrl = string.Format("BugEdit.aspx?pid={0}&id={1}", project.Id, bug.Id);
        linkAddNote.NavigateUrl = string.Format("BugNoteEdit.aspx?bid={0}", bug.Id);

        gridNotes_OnGetDataSource(this, null);
        gridNotes.DataBind();
    }

    void gridNotes_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            gridNotes.DataSource = BugService.GetBugNotes(RequestId);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    };

    public void gridNotes_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.AlternatingItem:
                case ListItemType.Item:
                case ListItemType.SelectedItem:
                case ListItemType.EditItem:
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    switch (e.CommandName)
                    {
                        case "Delete":
                            BugService.DeleteBugNote(SessionManager.Ticket, id);
                            ReportInfo("Note deleted.");
                            gridNotes.CurrentPageIndex = 0;
                            gridNotes_OnGetDataSource(source, e);
                            gridNotes.DataBind();
                            break;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkAction_Command(object sender, CommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Resolve":
                    Response.Redirect(string.Format("BugResolve.aspx?id={0}", RequestId));
                    break;
                case "Reopen":
                    BugService.ReopenBug(SessionManager.Ticket, RequestId);
                    break;
                case "Close":
                    BugService.CloseBug(SessionManager.Ticket, RequestId);
                    break;
            }

            GetBug();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkSwitchMode_Click(object sender, EventArgs e)
    {
        try
        {
            TransitBug bug = BugService.GetBugById(RequestId);
            switch(linkSwitchMode.CommandArgument)
            {
                case "Text":
                    linkSwitchMode.Text = "&#187; html";
                    linkSwitchMode.CommandArgument = "Html";
                    break;
                case "Html":
                    linkSwitchMode.Text = "&#187; text";
                    linkSwitchMode.CommandArgument = "Text";
                    break;
            }

            bugDetails.Text = RenderEx(bug.Details);
            gridNotes_OnGetDataSource(this, null);
            gridNotes.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public object RenderEx(object o)
    {
        if (o == null)
            return string.Empty;

        return RenderEx(o.ToString());
    }

    public string RenderEx(string s)
    {
        switch (linkSwitchMode.CommandArgument)
        {
            case "Text":
                return base.RenderEx(s);
                break;
            case "Html":
                return base.Render(s);
                break;
        }
        return string.Empty;
    }
}
