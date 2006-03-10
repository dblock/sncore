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

public partial class BugSeverityEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitBugSeverity t = BugService.GetBugSeverityById(RequestId);
                    inputName.Text = t.Name;
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
            TransitBugSeverity t = new TransitBugSeverity();
            t.Name = inputName.Text;
            t.Id = RequestId;
            BugService.CreateOrUpdateBugSeverity(SessionManager.Ticket, t);
            Redirect("BugSeveritiesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
