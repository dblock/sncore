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
            if (mAccountEventAccount == null && RequestId > 0 && AccountEvent != null)
            {
                object[] args = { AccountEvent.AccountId };
                mAccountEventAccount = SessionManager.GetCachedItem<TransitAccount>(
                    AccountService, "GetAccountById", args);
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
                mAccountEventPlace = PlaceService.GetPlaceById(AccountEvent.PlaceId);
            }
            return mAccountEventPlace;
        }
    }

    public string SuggestedBy
    {
        get
        {
            try
            {
                TransitAccount account = AccountEventAccount;

                if (account == null)
                    return string.Empty;

                return string.Format("posted by <a href='AccountView.aspx?id={0}'>{1}</a> on {2}",
                    account.Id,
                    Renderer.Render(account.Name),
                    base.Adjust(AccountEvent.Created).ToString("d"));
            }
            catch (Exception ex)
            {
                ReportException(ex);
                return string.Empty;
            }
        }
    }

    public TransitAccountEvent AccountEvent
    {
        get
        {
            if (mAccountEvent == null && RequestId > 0)
            {
                object[] args = { SessionManager.Ticket, RequestId, SessionManager.UtcOffset };
                mAccountEvent = SessionManager.GetCachedItem <TransitAccountEvent>(
                    EventService, "GetAccountEventById", args);
            }

            return mAccountEvent;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitAccountEvent evt = AccountEvent;

                    if (evt == null)
                    {
                        ReportWarning("AccountEvent does not exist.");
                        pnlAccountEvent.Visible = false;
                        return;
                    }

                    this.Title = Renderer.Render(evt.Name);

                    linkType.NavigateUrl = string.Format("AccountEventsView.aspx?city={0}&state={1}&country={2}&type={3}",
                        Renderer.UrlEncode(evt.PlaceCity),
                        Renderer.UrlEncode(evt.PlaceState),
                        Renderer.UrlEncode(evt.PlaceCountry),
                        Renderer.UrlEncode(evt.AccountEventType));

                    linkCity.NavigateUrl = string.Format("AccountEventsView.aspx?city={0}&state={1}&country={2}",
                        Renderer.UrlEncode(evt.PlaceCity),
                        Renderer.UrlEncode(evt.PlaceState),
                        Renderer.UrlEncode(evt.PlaceCountry));

                    linkState.NavigateUrl = string.Format("AccountEventsView.aspx?state={0}&country={1}",
                        Renderer.UrlEncode(evt.PlaceState),
                        Renderer.UrlEncode(evt.PlaceCountry));

                    linkCountry.NavigateUrl = string.Format("AccountEventsView.aspx?country={0}",
                        Renderer.UrlEncode(evt.PlaceCountry));

                    labelDescription.Text = base.RenderEx(evt.Description);
                    panelDescription.Visible = !string.IsNullOrEmpty(labelDescription.Text);
                    linkAccountEvent.Text = Renderer.Render(evt.Name);
                    linkCity.Text = AccountEventCity.Text = Renderer.Render(evt.PlaceCity);
                    linkState.Text = AccountEventState.Text = Renderer.Render(evt.PlaceState);
                    linkCountry.Text = AccountEventCountry.Text = Renderer.Render(evt.PlaceCountry);
                    linkType.Text = evt.AccountEventType + "s";
                    AccountEventName.Text = Renderer.Render(evt.Name);
                    AccountEventId.Text = "#" + evt.Id.ToString();

                    linkEdit.NavigateUrl = string.Format("AccountEventEdit.aspx?id={0}", evt.Id);

                    AccountEventWebsite.NavigateUrl = evt.Website;
                    AccountEventWebsite.Text = Renderer.Render(evt.Website);
                    AccountEventPhone.Text = Renderer.Render(evt.Phone);
                    AccountEventType.Text = Renderer.Render(evt.AccountEventType);
                    AccountEventEmail.OnClientClick = string.Format("location.href='mailto:{0}';",
                        Renderer.Render(evt.Email));
                    AccountEventEmail.Visible = ! string.IsNullOrEmpty(evt.Email);
                    AccountEventCost.Text = Renderer.Render(evt.Cost);

                    object[] p_args = { SessionManager.Ticket, RequestId };
                    picturesView.DataSource = SessionManager.GetCachedCollection<TransitAccountEventPicture>(
                        EventService, "GetAccountEventPictures", p_args);
                    picturesView.DataBind();

                    if (picturesView.Items.Count == 0)
                    {
                        panelNoPicture.Visible = true;
                    }

                    discussionAccountEvents.DiscussionId = DiscussionService.GetAccountEventDiscussionId(RequestId);
                    discussionAccountEvents.DataBind();

                    if (SessionManager.IsAdministrator)
                    {
                        linkFeature.Text = (LatestAccountEventFeature != null)
                            ? string.Format("Feature &#187; Last on {0}", Adjust(LatestAccountEventFeature.Created).ToString("d"))
                            : "Feature &#187; Never Featured";
                    }

                    panelOwner.Visible = SessionManager.IsLoggedIn &&
                        (SessionManager.IsAdministrator || evt.AccountId == SessionManager.Account.Id);

                    TransitPlace pl = AccountEventPlace;
                    placeImage.Src = string.Format("PlacePictureThumbnail.aspx?id={0}", pl.PictureId);
                    placeLink2.HRef = placeLink.HRef = string.Format("PlaceView.aspx?id={0}", pl.Id);
                    placeName.Text = Renderer.Render(pl.Name);
                    placeCity.Text = Renderer.Render(pl.City);
                    placeState.Text = Renderer.Render(pl.State);
                    placeCountry.Text = Renderer.Render(pl.Country);

                    labelSchedule.Text = Renderer.Render(evt.Schedule);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
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
        try
        {
            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to feature AccountEvents.");
            }

            TransitFeature t_feature = new TransitFeature();
            t_feature.DataObjectName = "AccountEvent";
            t_feature.DataRowId = RequestId;
            SystemService.CreateOrUpdateFeature(SessionManager.Ticket, t_feature);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to feature AccountEvents.");
            }

            TransitFeature t_feature = new TransitFeature();
            t_feature.DataObjectName = "AccountEvent";
            t_feature.DataRowId = RequestId;
            SystemService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public TransitFeature LatestAccountEventFeature
    {
        get
        {
            if (mAccountEventFeature == null)
            {
                mAccountEventFeature = SystemService.FindLatestFeature(
                    "AccountEvent", RequestId);
            }
            return mAccountEventFeature;
        }
    }

    public void linkExportVCalendar_Click(object sender, EventArgs e)
    {
        try
        {
            string vcsContent = EventService.GetAccountEventVCalendarById(SessionManager.Ticket, RequestId);
            Response.Clear(); // clear the current output content from the buffer
            Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.ics", AccountEvent.Name));
            Response.AppendHeader("Content-Length", vcsContent.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.Write(vcsContent);
            Response.End();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
