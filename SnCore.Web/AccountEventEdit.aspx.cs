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

public partial class AccountEventEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        place.Choose += new EventHandler(event_Changed);
        schedule.Confirm += new EventHandler(event_Changed);
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsManage.aspx"));

            ArrayList types = new ArrayList();
            types.Add(new TransitAccountPlaceType());
            types.AddRange(SessionManager.EventService.GetAccountEventTypes());
            selectType.DataSource = types;
            selectType.DataBind();

            int id = RequestId;

            if (id > 0)
            {
                TransitAccountEvent tav = SessionManager.EventService.GetAccountEventById(SessionManager.Ticket, id, SessionManager.UtcOffset);
                inputName.Text = tav.Name;
                inputWebsite.Text = tav.Website;
                inputDescription.Text = tav.Description;
                inputPhone.Text = tav.Phone;
                inputEmail.Text = tav.Email;
                inputCost.Text = tav.Cost;
                inputPublish.Checked = tav.Publish;
                selectType.Items.FindByValue(tav.AccountEventType).Selected = true;
                schedule.Schedule = SessionManager.SystemService.GetScheduleById(tav.ScheduleId);
                place.Place = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, tav.PlaceId);
                titleEvent.Text = Renderer.Render(tav.Name);
                sitemapdata.Add(new SiteMapDataAttributeNode(tav.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Event", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);

        if (!SessionManager.AccountService.HasVerifiedEmail(SessionManager.Ticket))
        {
            ReportWarning("You don't have any verified e-mail addresses.\n" +
                "You must add/confirm a valid e-mail address before posting.");

            manageAdd.Enabled = false;
        }
    }

    void event_Changed(object sender, EventArgs e)
    {
        panelReminder.Visible = true;
        panelReminderUpdate.Update();
    }

    public void save_Click(object sender, EventArgs e)
    {
        if (!schedule.IsConfirmed)
            schedule.ConfirmSchedule(sender, e);

        if (!place.IsChosen)
            place.SavePlace(sender, e);

        panelReminder.Visible = false;
        panelReminderUpdate.Update();

        TransitAccountEvent tav = new TransitAccountEvent();
        tav.Name = inputName.Text;

        if (!string.IsNullOrEmpty(inputWebsite.Text) && !Uri.IsWellFormedUriString(inputWebsite.Text, UriKind.Absolute))
            inputWebsite.Text = "http://" + inputWebsite.Text;

        if (string.IsNullOrEmpty(selectType.SelectedValue))
            throw new Exception("Please choose an event type.");

        if (!schedule.IsConfirmed)
            throw new Exception("Please confirm the schedule for this event.");

        if (!place.IsChosen)
            throw new Exception("Please choose a location where this event takes place.");

        tav.Website = inputWebsite.Text;
        tav.Description = inputDescription.Text;
        tav.Email = inputEmail.Text;
        tav.Cost = inputCost.Text;
        tav.Phone = inputPhone.Text;
        tav.Publish = inputPublish.Checked;

        // create place
        if (place.Place.Id == 0)
        {
            place.Place.Id = SessionManager.PlaceService.CreateOrUpdatePlace(
                SessionManager.Ticket, place.Place);
        }

        // create or update schedule
        tav.ScheduleId = schedule.Schedule.Id = SessionManager.SystemService.CreateOrUpdateSchedule(
            SessionManager.Ticket, schedule.Schedule);

        tav.AccountId = SessionManager.Account.Id;
        tav.PlaceId = place.Place.Id;
        tav.Id = RequestId;
        tav.AccountEventType = selectType.SelectedValue;
        SessionManager.EventService.CreateOrUpdateAccountEvent(SessionManager.Ticket, tav);
        Redirect("AccountEventsManage.aspx");

    }
}
