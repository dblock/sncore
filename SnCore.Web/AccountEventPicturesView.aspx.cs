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
                TransitAccountEvent a = SessionManager.EventService.GetAccountEventById(
                    SessionManager.Ticket, RequestId, SessionManager.UtcOffset);

                this.Title = string.Format("{0} Pictures", Renderer.Render(a.Name));
                listView.DataSource = SessionManager.EventService.GetAccountEventPictures(
                    SessionManager.Ticket, RequestId, null);
                listView.DataBind();

                sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "AccountEventsToday.aspx", a.PlaceCountry, a.PlaceState, a.PlaceCity, a.PlaceNeighborhood, a.AccountEventType));
                sitemapdata.Add(new SiteMapDataAttributeNode(a.Name, Request, string.Format("AccountEventView.aspx?id={0}", a.Id)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }
    }
}
