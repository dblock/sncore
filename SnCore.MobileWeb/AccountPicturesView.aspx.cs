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
using AccountService;

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
            TransitAccount a = SessionManager.GetInstance<TransitAccount, AccountService.ServiceQueryOptions, int>(
                RequestId, SessionManager.AccountService.GetAccountById);

            accountName.Text = this.Title = string.Format("{0}'s Pictures", Renderer.Render(a.Name));
            linkBack.NavigateUrl = string.Format("AccountView.aspx?id={0}", a.Id);

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
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountPicture, AccountService.ServiceQueryOptions, int, AccountPicturesQueryOptions>(
            RequestId, ap, SessionManager.AccountService.GetAccountPicturesCount);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        AccountPicturesQueryOptions ap = new AccountPicturesQueryOptions();
        ap.Hidden = false;
        AccountService.ServiceQueryOptions options = new AccountService.ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountPicture, AccountService.ServiceQueryOptions, int, AccountPicturesQueryOptions>(
            RequestId, ap, options, SessionManager.AccountService.GetAccountPictures);
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
