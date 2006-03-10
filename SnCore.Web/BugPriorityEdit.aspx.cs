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

public partial class BugPriorityEdit : AuthenticatedPage
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
                    TransitBugPriority t = BugService.GetBugPriorityById(RequestId);
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
            TransitBugPriority t = new TransitBugPriority();
            t.Name = inputName.Text;
            t.Id = RequestId;
            BugService.CreateOrUpdateBugPriority(SessionManager.Ticket, t);
            Redirect("BugPrioritiesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
