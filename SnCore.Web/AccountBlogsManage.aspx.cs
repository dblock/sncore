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
using SnCore.SiteMap;

public partial class AccountBlogsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            gridManageAuthor.OnGetDataSource += new EventHandler(gridManageAuthor_OnGetDataSource);

            if (!IsPostBack)
            {
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();

                gridManageAuthor_OnGetDataSource(this, null);
                gridManageAuthor.DataBind();

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request.Url));
                StackSiteMap(sitemapdata);
            }
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

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            gridManage.DataSource = BlogService.GetAccountBlogs(SessionManager.Ticket);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManageAuthor_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            gridManageAuthor.DataSource = BlogService.GetAccountBlogAuthored(SessionManager.Ticket);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
            switch (e.CommandName)
            {
                case "Delete":
                    BlogService.DeleteAccountBlog(SessionManager.Ticket, id);
                    ReportInfo("Blog deleted.");
                    gridManage.CurrentPageIndex = 0;
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
