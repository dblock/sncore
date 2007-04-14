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
using System.Collections.Generic;

public partial class AccountRssWatchView : Page
{
    private TransitAccountRssWatch mAccountRssWatch = null;

    public TransitAccountRssWatch AccountRssWatch
    {
        get
        {
            if (mAccountRssWatch == null)
            {
                mAccountRssWatch = SessionManager.GetInstance<TransitAccountRssWatch, int>(
                    RequestId, SessionManager.SyndicationService.GetAccountRssWatchById);
            }
            return mAccountRssWatch;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            TransitAccountRssWatch f = AccountRssWatch;
            labelTitle.Text = Renderer.Render(f.Name);
            linkEditRssWatch.NavigateUrl = string.Format("AccountRssWatchEdit.aspx?id={0}", f.Id);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Subscriptions", Request, "AccountRssWatchsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(f.Name, Request.Url));
            StackSiteMap(sitemapdata);

            GetData(sender, e);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        List<TransitRssItem> items = SessionManager.SyndicationService.GetAccountRssWatchItems(
            SessionManager.Ticket, RequestId, null);
        gridManage.VirtualItemCount = items.Count;
        if (items.Count > gridManage.PageSize) 
            items.RemoveRange(gridManage.PageSize, items.Count - gridManage.PageSize);
        gridManage.DataSource = items;
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitRssItem, int>(
            RequestId, options, SessionManager.SyndicationService.GetAccountRssWatchItems);
    }
}
