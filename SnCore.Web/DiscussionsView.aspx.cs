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
using SnCore.WebServices;
using SnCore.SiteMap;

[SiteMapDataAttribute("Discussions")]
public partial class DiscussionsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(search);
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.DiscussionService.GetDiscussionsCount(
            SessionManager.Ticket);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
        gridManage.DataSource = SessionManager.DiscussionService.GetDiscussions(
            SessionManager.Ticket, options);
    }

    protected void search_Click(object sender, EventArgs e)
    {
        Redirect("SearchDiscussionPosts.aspx?q=" + Renderer.UrlEncode(inputSearch.Text));
    }
}
