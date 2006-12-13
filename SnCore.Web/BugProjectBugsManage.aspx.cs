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
using SnCore.WebServices;
using SnCore.SiteMap;

public partial class BugProjectBugsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                linkNew.NavigateUrl = string.Format("BugEdit.aspx?pid={0}", RequestId);
                TransitBugProject p = BugService.GetBugProjectById(RequestId);
                this.Title = string.Format("{0} Bugs", Render(p.Name));
                gridManage.SortDirection = PagedGridSortDirection.Descending;
                gridManage.SortExpression = "Created";
                GetData(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Bugs", Request, "BugProjectsManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request.Url));
                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(search);
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
            GetData(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = BugService.GetBugsCount(GetQueryOptions());
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    TransitBugQueryOptions GetQueryOptions()
    {
        TransitBugQueryOptions qo = new TransitBugQueryOptions();
        qo.ProjectId = RequestId;
        qo.SortDirection =
            (gridManage.SortDirection == PagedGridSortDirection.Ascending)
                ? TransitSortDirection.Ascending
                : TransitSortDirection.Descending;
        qo.SortExpression = gridManage.SortExpression;
        qo.Resolved = checkboxResolvedBugs.Checked;
        qo.Open = checkboxOpenedBugs.Checked;
        qo.Closed = checkboxClosedBugs.Checked;
        qo.SearchQuery = inputSearch.Text;
        return qo;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            
            gridManage.DataSource = BugService.GetBugs(GetQueryOptions(), options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void search_Click(object sender, EventArgs e)
    {
        try
        {
            GetData(sender, e);
            panelBugs.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        try
        {
            panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
            if (!panelSearchInternal.PersistentVisible) inputSearch.Text = string.Empty;
            panelSearch.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
