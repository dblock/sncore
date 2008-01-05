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
using System.Text;
using SnCore.SiteMap;

public partial class AccountEventView : Page
{
    private TransitAccountEvent mAccountEvent = null;
    private TransitPlace mAccountEventPlace = null;
    private TransitFeature mAccountEventFeature = null;
    private TransitAccount mAccountEventAccount = null;

    public TransitAccount AccountEventAccount
    {
        get
        {
            try
            {
                if (mAccountEventAccount == null && RequestId > 0 && AccountEvent != null)
                {
                    mAccountEventAccount = SessionManager.GetInstance<TransitAccount, int>(
                        AccountEvent.AccountId, SessionManager.AccountService.GetAccountById);
                }
            }
            catch
            {
            }

            return mAccountEventAccount;
        }
    }

    public TransitPlace AccountEventPlace
    {
        get
        {
            if (mAccountEventPlace == null && RequestId > 0 && AccountEvent != null)
            {
                mAccountEventPlace = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, AccountEvent.PlaceId);
            }
            return mAccountEventPlace;
        }
    }

    public string SuggestedBy
    {
        get
        {
            TransitAccount account = AccountEventAccount;

            if (account == null)
                return string.Empty;

            return string.Format("posted by <a href='AccountView.aspx?id={0}'>{1}</a> on {2}",
                account.Id,
                Renderer.Render(account.Name),
                base.Adjust(AccountEvent.Created).ToString("d"));
        }
    }

    public TransitAccountEvent AccountEvent
    {
        get
        {
            try
            {
                if (mAccountEvent == null && RequestId > 0)
                {
                    mAccountEvent = SessionManager.GetInstance<TransitAccountEvent, int, int>(
                        RequestId, SessionManager.UtcOffset, SessionManager.EventService.GetAccountEventById);
                }
            }
            catch
            {

            }

            return mAccountEvent;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (RequestId > 0)
            {
                TransitAccountEvent t = AccountEvent;

                if (t == null)
                {
                    ReportWarning("Event does not exist.");
                    pnlAccountEvent.Visible = false;
                    return;
                }

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsView.aspx"));
                sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "AccountEventsToday.aspx", t.PlaceCountry, t.PlaceState, t.PlaceCity, t.PlaceNeighborhood, t.AccountEventType));
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
                StackSiteMap(sitemapdata);

                picturesView.AccountEventId = RequestId;

                this.Title = Renderer.Render(t.Name);

                labelDescription.Text = base.RenderEx(t.Description);
                panelDescription.Visible = !string.IsNullOrEmpty(labelDescription.Text);
                eventName.Text = Renderer.Render(t.Name);
                eventId.Text = "#" + t.Id.ToString();

                linkEdit.NavigateUrl = string.Format("AccountEventEdit.aspx?id={0}", t.Id);

                eventWebsite.NavigateUrl = t.Website;
                if (!string.IsNullOrEmpty(t.Website))
                {
                    eventWebsite.NavigateUrl = t.Website;
                    if (t.Website.Length < 64)
                    {
                        eventWebsite.Text = Renderer.Render(t.Website);
                    }
                }

                eventWebsite.Visible = ! string.IsNullOrEmpty(t.Website);
                eventPhone.Text = Renderer.Render(t.Phone);
                eventType.Text = Renderer.Render(t.AccountEventType);
                eventEmail.NavigateUrl = string.Format("mailto:{0}", Renderer.Render(t.Email));
                eventEmail.Visible = !string.IsNullOrEmpty(t.Email);
                eventCost.Text = Renderer.Render(t.Cost);

                discussionAccountEvents.DiscussionId = SessionManager.GetCount<TransitDiscussion, string, int>(
                    typeof(AccountEvent).Name, RequestId, SessionManager.DiscussionService.GetOrCreateDiscussionId);
                discussionAccountEvents.DataBind();

                if (SessionManager.IsAdministrator)
                {
                    linkFeature.Text = (LatestAccountEventFeature != null)
                        ? string.Format("Feature &#187; Last on {0}", Adjust(LatestAccountEventFeature.Created).ToString("d"))
                        : "Feature &#187; Never Featured";
                }

                panelOwner.Visible = SessionManager.IsLoggedIn &&
                    (SessionManager.IsAdministrator || t.AccountId == SessionManager.Account.Id);

                TransitPlace pl = AccountEventPlace;
                placeImage.Src = string.Format("PlacePictureThumbnail.aspx?id={0}", pl.PictureId);
                placeLink2.HRef = placeLink.HRef = string.Format("PlaceView.aspx?id={0}", pl.Id);
                placeNeighborhood.Text = Renderer.Render(pl.Neighborhood);
                placeName.Text = Renderer.Render(pl.Name);
                placeCity.Text = Renderer.Render(pl.City);
                placeState.Text = Renderer.Render(pl.State);
                placeCountry.Text = Renderer.Render(pl.Country);

                labelSchedule.Text = Renderer.Render(t.Schedule);
            }
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        panelAdmin.Visible = SessionManager.IsAdministrator;
        linkDeleteFeatures.Visible = (LatestAccountEventFeature != null);
        base.OnPreRender(e);
    }

    public void feature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature AccountEvents.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountEvent";
        t_feature.DataRowId = RequestId;
        SessionManager.CreateOrUpdate<TransitFeature>(
            t_feature, SessionManager.ObjectService.CreateOrUpdateFeature);
        Redirect(Request.Url.PathAndQuery);
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature events.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountEvent";
        t_feature.DataRowId = RequestId;
        SessionManager.ObjectService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
        SessionManager.InvalidateCache<TransitFeature>();
        Redirect(Request.Url.PathAndQuery);
    }

    public TransitFeature LatestAccountEventFeature
    {
        get
        {
            if (mAccountEventFeature == null)
            {
                mAccountEventFeature = SessionManager.GetInstance<TransitFeature, string, int>(
                    "AccountEvent", RequestId, SessionManager.ObjectService.FindLatestFeature);
            }
            return mAccountEventFeature;
        }
    }

    public void linkExportVCalendar_Click(object sender, EventArgs e)
    {
        string vcsContent = SessionManager.EventService.GetAccountEventVCalendarById(SessionManager.Ticket, RequestId);
        Response.Clear(); // clear the current output content from the buffer
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.ics", AccountEvent.Name));
        Response.AppendHeader("Content-Length", vcsContent.Length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.Write(vcsContent);
        Response.End();
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        SessionManager.Delete<TransitAccountEvent>(RequestId, SessionManager.EventService.DeleteAccountEvent);
        SessionManager.InvalidateCache<TransitAccountEventInstance>();
        Redirect("AccountEventsToday.aspx");
    }
}
