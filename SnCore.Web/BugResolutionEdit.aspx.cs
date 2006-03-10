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

public partial class BugResolutionEdit : AuthenticatedPage
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
                    TransitBugResolution t = BugService.GetBugResolutionById(RequestId);
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
            TransitBugResolution t = new TransitBugResolution();
            t.Name = inputName.Text;
            t.Id = RequestId;
            BugService.CreateOrUpdateBugResolution(SessionManager.Ticket, t);
            Redirect("BugResolutionsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
