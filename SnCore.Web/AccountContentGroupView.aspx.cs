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
using SnCore.SiteMap;

public partial class AccountContentGroupView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            TransitAccountContentGroup group = SessionManager.ContentService.GetAccountContentGroupById(
                SessionManager.Ticket, RequestId);

            if (group == null)
            {
                Response.StatusCode = 404;
                Response.End();
                return;
            }

            if (group.Login && !SessionManager.IsLoggedIn)
            {
                RedirectToLogin();
            }

            linkRelRss.Title = Title = labelName.Text = Renderer.Render(group.Name);
            labelDescription.Text = Renderer.Render(group.Description);

            linkRelRss.NavigateUrl = string.Format("AccountContentGroupViewRss.aspx?id={0}", RequestId);

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode(group.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.ContentService.GetAccountContentsCount(
            SessionManager.Ticket, RequestId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.ContentService.GetAccountContents(
            SessionManager.Ticket, RequestId, options);
    }
}
