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

public partial class AccountEventPicturesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsToday.aspx"));

            if (RequestId > 0)
            {
                TransitAccountEvent evt = SessionManager.EventService.GetAccountEventById(
                    SessionManager.Ticket, RequestId, SessionManager.UtcOffset);

                this.Title = string.Format("{0} Pictures", Renderer.Render(evt.Name));
                listView.DataSource = SessionManager.EventService.GetAccountEventPictures(
                    SessionManager.Ticket, RequestId, null);
                listView.DataBind();

                sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "AccountEventsToday.aspx", evt.PlaceCountry, evt.PlaceState, evt.PlaceCity, evt.PlaceNeighborhood, evt.AccountEventType));
                sitemapdata.Add(new SiteMapDataAttributeNode(evt.Name, Request, string.Format("AccountEventView.aspx?id={0}", evt.Id)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
