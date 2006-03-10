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

public partial class BugResolve : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            linkBack.NavigateUrl = string.Format("BugView.aspx?id={0}", RequestId);
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                selectResolution.DataSource = BugService.GetBugResolutions();
                selectResolution.DataBind();
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
            BugService.ResolveBug(SessionManager.Ticket, RequestId, selectResolution.SelectedValue, inputNote.Text);
            Redirect(string.Format("BugView.aspx?id={0}", RequestId));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
