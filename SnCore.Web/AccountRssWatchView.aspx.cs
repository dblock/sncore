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
            Title = labelTitle.Text = Renderer.Render(f.Name);
            linkEditRssWatch.NavigateUrl = string.Format("AccountRssWatchEdit.aspx?id={0}", f.Id);
            labelSince.Text = string.Format("since {0}", SessionManager.Adjust(f.Sent).ToShortDateString());

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
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.SyndicationService.GetAccountRssWatchItemsCount(
            SessionManager.Ticket, RequestId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        
        TransitRssChannelItems items = SessionManager.SyndicationService.GetAccountRssWatchItems(
            SessionManager.Ticket, RequestId, options);
        
        if (!string.IsNullOrEmpty(items.Channel.Title))
        {
            linkChannel.Text = string.Format("&#187; {0}", Renderer.Render(items.Channel.Title));
            linkChannel.NavigateUrl = items.Channel.Link.ToString();
        }

        gridManage.DataSource = items.Items;
    }
}
