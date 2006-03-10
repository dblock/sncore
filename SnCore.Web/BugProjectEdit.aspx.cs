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

public partial class BugProjectEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetDefaultButton(manageAdd);
                if (RequestId > 0)
                {
                    TransitBugProject t = BugService.GetBugProjectById(RequestId);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
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
            TransitBugProject t = new TransitBugProject();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.Id = RequestId;
            BugService.CreateOrUpdateBugProject(SessionManager.Ticket, t);
            Redirect("BugProjectsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
