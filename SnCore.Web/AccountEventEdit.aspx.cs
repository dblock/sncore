using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using System.Net;
using System.IO;
using SnCore.Data.Hibernate;

public partial class AccountEventEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        place.Choose += new EventHandler(event_Changed);
        schedule.Confirm += new EventHandler(event_Changed);
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("AccountEvent");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputPhone.MaxLength = cs["Phone"].MaxLengthInChars;
            inputEmail.MaxLength = cs["Email"].MaxLengthInChars;
            inputWebsite.MaxLength = cs["Website"].MaxLengthInChars;
            inputCost.MaxLength = cs["Cost"].MaxLengthInChars;

            GetAccountEventTypes(sender, e);

            linkBack.NavigateUrl = ReturnUrl;

            if (RequestId > 0)
            {
                TransitAccountEvent tav = SessionManager.EventService.GetAccountEventById(
                    SessionManager.Ticket, RequestId, SessionManager.UtcOffset);
                inputName.Text = tav.Name;
                inputWebsite.Text = tav.Website;
                inputDescription.Text = tav.Description;
                inputPhone.Text = tav.Phone;
                inputEmail.Text = tav.Email;
                inputCost.Text = tav.Cost;
                inputPublish.Checked = tav.Publish;
                selectType.ClearSelection();
                selectType.Items.FindByValue(tav.AccountEventType).Selected = true;
                schedule.Schedule = SessionManager.ObjectService.GetScheduleById(SessionManager.Ticket, tav.ScheduleId);
                place.Place = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, tav.PlaceId);
                titleEvent.Text = Renderer.Render(tav.Name);
                sitemapdata.Add(new SiteMapDataAttributeNode(tav.Name, Request.Url));
            }
            else if (!string.IsNullOrEmpty(Request["ical"]))
            {
                TransitAccountEventICALEmitter emitter = TransitAccountEventICALEmitter.Parse(
                    Request["ical"], SessionManager.UtcOffset, SessionManager.GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0"));
                inputName.Text = emitter.AccountEvent.Name;
                inputWebsite.Text = emitter.AccountEvent.Website;
                inputDescription.Text = emitter.AccountEvent.Description;
                inputPhone.Text = emitter.AccountEvent.Phone;
                inputEmail.Text = emitter.AccountEvent.Email;
                inputCost.Text = emitter.AccountEvent.Cost;
                inputPublish.Checked = emitter.AccountEvent.Publish;
                if (!string.IsNullOrEmpty(emitter.AccountEvent.AccountEventType))
                {
                    selectType.ClearSelection();
                    selectType.Items.FindByValue(emitter.AccountEvent.AccountEventType).Selected = true;
                }
                schedule.Schedule = emitter.Schedule;
                place.Place = emitter.Place;
                titleEvent.Text = Renderer.Render(emitter.AccountEvent.Name);
                sitemapdata.Add(new SiteMapDataAttributeNode("New ICal Event", Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Event", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);

        if (!SessionManager.HasVerifiedEmailAddress())
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
            throw new Exception("Please choose a type for this event.");

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
            place.Place.Id = SessionManager.CreateOrUpdate<TransitPlace>(
                place.Place, SessionManager.PlaceService.CreateOrUpdatePlace);
        }

        // create or update schedule
        tav.ScheduleId = schedule.Schedule.Id = SessionManager.CreateOrUpdate<TransitSchedule>(
            schedule.Schedule, SessionManager.ObjectService.CreateOrUpdateSchedule);

        tav.AccountId = SessionManager.Account.Id;
        tav.PlaceId = place.Place.Id;
        tav.Id = RequestId;
        tav.AccountEventType = selectType.SelectedValue;
        tav.Id = SessionManager.CreateOrUpdate<TransitAccountEvent>(
            tav, SessionManager.EventService.CreateOrUpdateAccountEvent);
        Redirect(string.Format("AccountEventView.aspx?id={0}", tav.Id));
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "AccountEventsToday.aspx" : o.ToString());
        }
    }

    private void GetAccountEventTypes(object sender, EventArgs e)
    {
        List<TransitAccountEventType> types = SessionManager.EventService.GetAccountEventTypes(SessionManager.Ticket, null);

        TransitAccountEventType selected = null;
        foreach (TransitAccountEventType eventtype in types)
        {
            if (eventtype.DefaultType)
            {
                selected = eventtype;
                break;
            }
        }

        if (selected == null)
        {
            types.Insert(0, new TransitAccountEventType());
        }

        selectType.DataSource = types;
        selectType.DataBind();

        if (selected != null)
        {
            selectType.ClearSelection();
            selectType.Items.FindByValue(selected.Name).Selected = true;
        }
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        SessionManager.Delete<TransitAccountEvent>(RequestId, SessionManager.EventService.DeleteAccountEvent);
        Redirect("AccountEventsToday.aspx");
    }
}
