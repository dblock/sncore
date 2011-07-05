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

public partial class AccountPlaceFavoritesView : AccountPersonPage
{
    public int RequestAccountId
    {
        get
        {
            return RequestId > 0 ? RequestId : SessionManager.Account.Id;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            TransitAccount ta = SessionManager.GetInstance<TransitAccount, int>(
                RequestAccountId, SessionManager.AccountService.GetAccountById);

            labelName.Text = string.Format("{0}'s Favorite Places", Render(ta.Name));
            linkAccount.Text = string.Format("&#187; Back to {0}", Render(ta.Name));
            linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", ta.Id);
            linkRelRss.NavigateUrl = string.Format("AccountPlaceFavoritesRss.aspx?id={0}", ta.Id);
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Foodies", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(ta.Name, Request, string.Format("AccountView.aspx?id={0}", ta.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Favorite Places", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountPlaceFavorite, int>(
            RequestAccountId, SessionManager.PlaceService.GetAccountPlaceFavoritesCountByAccountId);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountPlaceFavorite, int>(
            RequestAccountId, options, SessionManager.PlaceService.GetAccountPlaceFavoritesByAccountId);
    }
}
