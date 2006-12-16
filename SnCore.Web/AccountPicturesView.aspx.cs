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

public partial class AccountPicturesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (RequestId == 0)
        {
            throw new Exception("Missing account.");
        }

        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            TransitAccount a = SessionManager.AccountService.GetAccountById(RequestId);
            this.Title = string.Format("{0}'s Pictures", Renderer.Render(a.Name));

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(a.Name, Request, string.Format("AccountView.aspx?id={0}", a.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        AccountPicturesQueryOptions ap = new AccountPicturesQueryOptions();
        ap.Hidden = false;
        object[] p_args = { RequestId, ap };
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            SessionManager.AccountService, "GetAccountPicturesCountById", p_args);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        AccountPicturesQueryOptions ap = new AccountPicturesQueryOptions();
        ap.Hidden = false;
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        object[] args = { RequestId, ap, options };
        gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountPicture>(
            SessionManager.AccountService, "GetAccountPicturesById", args);
    }

}
