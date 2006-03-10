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
using SnCore.WebControls;
using SnCore.Services;

public partial class BugProjectBugsManage : AuthenticatedPage
{
    public void Page_Load()
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                linkNew.NavigateUrl = string.Format("BugEdit.aspx?pid={0}", RequestId);
                TransitBugProject p = BugService.GetBugProjectById(RequestId);
                linkSection.Text = Render(p.Name);
                this.Title = string.Format("{0} Bugs", Render(p.Name));
                gridManage.SortDirection = PagedGridSortDirection.Descending;
                gridManage.SortExpression = "Created";
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void optionsChanged(object sender, EventArgs e)
    {
        try
        {
            gridManage.CurrentPageIndex = 0;
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            TransitBugQueryOptions options = new TransitBugQueryOptions();
            options.SortDirection =
                (gridManage.SortDirection == PagedGridSortDirection.Ascending)
                    ? TransitSortDirection.Ascending
                    : TransitSortDirection.Descending;
            options.SortExpression = gridManage.SortExpression;
            options.Resolved = checkboxResolvedBugs.Checked;
            gridManage.DataSource = BugService.GetBugs(RequestId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }



}
