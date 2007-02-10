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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountPlaceQueueManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        queue.OnGetDataSource += new EventHandler(queue_OnGetDataSource);

        if (!IsPostBack)
        {
            if (RequestId == 0)
            {
                TransitPlaceQueue q = SessionManager.PlaceService.GetOrCreatePlaceQueueByName(
                    SessionManager.Ticket, SessionManager.AccountId, "My Queue");

                Response.Redirect(string.Format("AccountPlaceQueueManage.aspx?id={0}", q.Id));
                return;
            }

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "AccountPlacesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Queue", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        queue.CurrentPageIndex = 0;
        queue.VirtualItemCount = SessionManager.PlaceService.GetPlaceQueueItemsCount(SessionManager.Ticket, RequestId);
        queue_OnGetDataSource(this, null);
        queue.DataBind();
    }

    void queue_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = queue.CurrentPageIndex;
        options.PageSize = queue.PageSize;
        queue.DataSource = SessionManager.PlaceService.GetPlaceQueueItems(SessionManager.Ticket, RequestId, options);
    }

    public void queue_Command(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.CommandArgument.ToString());
                SessionManager.Delete<TransitPlaceQueueItem>(id, SessionManager.PlaceService.DeletePlaceQueueItem);
                ReportInfo("Place removed from queue.");
                GetData(source, e);
                break;
        }
    }
}
