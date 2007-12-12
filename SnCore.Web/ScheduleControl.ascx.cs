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
using SnCore.Services;
using System.Text;
using Wilco.Web.UI;
using SnCore.WebControls;

public partial class ScheduleControl : Control
{
    public event EventHandler Confirm;

    public enum HighLevelRecurrencePattern
    {
        None,
        Daily,
        Weekly,
        Monthly,
        Yearly
    };

    public HighLevelRecurrencePattern SelectedHighLevelRecurrencePattern
    {
        get
        {
            if (string.IsNullOrEmpty(recPattern.SelectedValue))
                return HighLevelRecurrencePattern.None;

            if (panelStandard.PersistentVisible)
                return HighLevelRecurrencePattern.None;

            return (HighLevelRecurrencePattern)Enum.Parse(
                typeof(HighLevelRecurrencePattern), recPattern.SelectedValue);
        }
    }

    public RecurrencePattern SelectedRecurrencePattern
    {
        get
        {
            HighLevelRecurrencePattern hrp = SelectedHighLevelRecurrencePattern;

            switch (hrp)
            {
                case HighLevelRecurrencePattern.Daily:
                    if (recDaily_EveryWeekday.Checked)
                        return RecurrencePattern.Daily_EveryWeekday;
                    if (recDaily_EveryNDays.Checked)
                        return RecurrencePattern.Daily_EveryNDays;
                    break;
                case HighLevelRecurrencePattern.Monthly:
                    if (recMonthly_DayNOfEveryNMonths.Checked)
                        return RecurrencePattern.Monthly_DayNOfEveryNMonths;
                    if (recMonthly_NthWeekDayOfEveryNMonth.Checked)
                        return RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth;
                    break;
                case HighLevelRecurrencePattern.Weekly:
                    return RecurrencePattern.Weekly;
                case HighLevelRecurrencePattern.Yearly:
                    if (recYearly_DayNOfMonth.Checked)
                        return RecurrencePattern.Yearly_DayNOfMonth;
                    if (recYearly_NthWeekDayOfMonth.Checked)
                        return RecurrencePattern.Yearly_NthWeekDayOfMonth;
                    break;
            }

            return RecurrencePattern.None;
        }
    }

    public TransitSchedule Schedule
    {
        get
        {
            TransitSchedule result = (TransitSchedule)ViewState["Schedule"];
            if (result == null)
            {
                DateTime at = DateTime.UtcNow;
                result = new TransitSchedule();

                result.StartDateTime = at;
                result.EndDateTime = at.AddHours(1);

                result.WeeklyDaysOfWeek = (short)Math.Pow(2, (int)at.DayOfWeek);

                result.MonthlyDay = at.Day;
                result.MonthlyExDayName = (int)at.DayOfWeek;

                result.YearlyMonth = (int)at.Month;
                result.YearlyExDayName = (int)at.DayOfWeek;
                result.YearlyExMonth = (int)at.Month;

                result.NoEndDateTime = true;

                ViewState["Schedule"] = result;
            }
            return result;
        }
        set
        {
            ViewState["Schedule"] = value;
        }
    }

    public bool IsConfirmed
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "IsConfirmed", false);
        }
        set
        {
            ViewState["IsConfirmed"] = value;
        }
    }


    protected void SelectRecurrencePattern()
    {
        recPattern.ClearSelection();
        switch (Schedule.RecurrencePattern)
        {
            case RecurrencePattern.Daily_EveryNDays:
                recPattern.SelectedValue = HighLevelRecurrencePattern.Daily.ToString();
                recDaily_EveryNDays.Checked = true;
                recDaily_EveryWeekday.Checked = false;
                break;
            case RecurrencePattern.Daily_EveryWeekday:
                recPattern.SelectedValue = HighLevelRecurrencePattern.Daily.ToString();
                recDaily_EveryNDays.Checked = false;
                recDaily_EveryWeekday.Checked = true;
                break;
            case RecurrencePattern.Weekly:
                recPattern.SelectedValue = HighLevelRecurrencePattern.Weekly.ToString();
                break;
            case RecurrencePattern.Monthly_DayNOfEveryNMonths:
                recPattern.SelectedValue = HighLevelRecurrencePattern.Monthly.ToString();
                recMonthly_DayNOfEveryNMonths.Checked = true;
                recMonthly_NthWeekDayOfEveryNMonth.Checked = false;
                break;
            case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
                recPattern.SelectedValue = HighLevelRecurrencePattern.Monthly.ToString();
                recMonthly_DayNOfEveryNMonths.Checked = false;
                recMonthly_NthWeekDayOfEveryNMonth.Checked = true;
                break;
            case RecurrencePattern.Yearly_DayNOfMonth:
                recPattern.SelectedValue = HighLevelRecurrencePattern.Yearly.ToString();
                recYearly_DayNOfMonth.Checked = true;
                recYearly_NthWeekDayOfMonth.Checked = false;
                break;
            case RecurrencePattern.Yearly_NthWeekDayOfMonth:
                recPattern.SelectedValue = HighLevelRecurrencePattern.Yearly.ToString();
                recYearly_DayNOfMonth.Checked = false;
                recYearly_NthWeekDayOfMonth.Checked = true;
                break;
        }
    }

    protected void SelectDaily()
    {
        recDailyEveryNDays.Text = Schedule.DailyEveryNDays.ToString();
    }

    protected void SelectWeekly()
    {
        recWeeklyEveryNWeeks.Text = Schedule.WeeklyEveryNWeeks.ToString();

        for (int i = 0; i < 7; i++)
        {
            if ((Schedule.WeeklyDaysOfWeek & (short)Math.Pow(2, i)) > 0)
            {
                recWeeklyDaysOfWeek.Items.FindByValue(i.ToString()).Selected = true;
            }
        }
    }

    protected void SelectMonthly()
    {
        recMonthlyDay.Text = Schedule.MonthlyDay.ToString();
        recMonthlyMonth.Text = Schedule.MonthlyMonth.ToString();
        ListItemManager.TrySelect(recMonthlyDayIndex, Schedule.MonthlyExDayIndex);
        ListItemManager.TrySelect(recMonthlyDayName, Schedule.MonthlyExDayName);
        recMonthlyExMonth.Text = Schedule.MonthlyExMonth.ToString();
    }

    protected void SelectYearly()
    {
        ListItemManager.TrySelect(recYearlyMonth, Schedule.YearlyMonth);

        recYearlyDay.Text = Schedule.YearlyDay.ToString();

        ListItemManager.TrySelect(recYearlyExDayName, Schedule.YearlyExDayName);
        ListItemManager.TrySelect(recYearlyExDayIndex, Schedule.YearlyExDayIndex);
        ListItemManager.TrySelect(recYearlyExMonth, Schedule.YearlyExMonth);
    }

    protected void SelectRange()
    {
        recStartDate.SelectedDate = base.ToUTC(Schedule.StartDateTime);
        recEndByDate.SelectedDate = base.ToUTC(Schedule.EndDateTime);
        recEndAfterNOccurences.Text = Schedule.EndOccurrences.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            stdStartDate.SelectedDate = base.Adjust(Schedule.StartDateTime);
            stdStartTime.SelectedTime = base.Adjust(Schedule.StartDateTime).TimeOfDay;

            stdEndDate.SelectedDate = base.Adjust(Schedule.EndDateTime);
            stdEndTime.SelectedTime = base.Adjust(Schedule.EndDateTime).TimeOfDay;

            stdAllDay.Checked = Schedule.AllDay;

            recStartTime.SelectedTime = base.Adjust(Schedule.StartDateTime).TimeOfDay;
            recEndTime.SelectedTime = base.Adjust(Schedule.EndDateTime).TimeOfDay;

            stdNoEndTime.Checked = recNoEndTime.Checked = Schedule.NoEndDateTime;

            UpdateSelection();

            if (Schedule.Id != 0)
            {
                labelConfirmed.Text = this.ToString();
                addRecurrent.Enabled = true;
                addOneTime.Enabled = true;
                editCurrent.Enabled = true;
                panelConfirmed.Visible = true;
                panelSchedule.Visible = false;
                IsConfirmed = true;
            }
        }

        UpdateEvents();
    }

    private void UpdateEvents()
    {
        StringBuilder changeclick = new StringBuilder();
        changeclick.Append("if (this.href != '') {");
        changeclick.Append("this.disabled = true;");
        changeclick.AppendFormat("document.getElementById('{0}').style.cssText = 'display: none';", panelButtons.ClientID);
        changeclick.AppendFormat("document.getElementById('{0}').disabled = 'disabled';", panelSchedule.ClientID);
        changeclick.AppendFormat("document.getElementById('{0}').innerText = '» please wait while I update the schedule ...'", panelWorking.ClientID);
        changeclick.Append("}");

        stdAllDay.Attributes.Add("onclick", changeclick.ToString());
        stdNoEndTime.Attributes.Add("onclick", changeclick.ToString());
        recNoEndTime.Attributes.Add("onclick", changeclick.ToString());

        addOneTime.OnClientClick = changeclick.ToString();
        addRecurrent.OnClientClick = changeclick.ToString();
        editCurrent.OnClientClick = changeclick.ToString();
    }

    public void UpdateSelection()
    {
        SelectRecurrencePattern();
        SelectDaily();
        SelectWeekly();
        SelectMonthly();
        SelectYearly();
        SelectRange();
    }

    public void VerifySchedule()
    {
        switch (Schedule.RecurrencePattern)
        {
            case RecurrencePattern.None:
                if (Schedule.AllDay)
                {
                    if (Schedule.EndDateTime.Date < Schedule.StartDateTime.Date)
                    {
                        throw new Exception("End date must be same or after start date.");
                    }
                }
                else if (Schedule.NoEndDateTime)
                {
                    
                }
                else
                {
                    if (Schedule.EndDateTime <= Schedule.StartDateTime)
                    {
                        throw new Exception(string.Format("End date/time ({0}) must be after start date/time ({1}).",
                            Schedule.EndDateTime, Schedule.StartDateTime));
                    }
                }
                break;
        }
    }

    public void UpdateSchedule()
    {
        Schedule.RecurrencePattern = SelectedRecurrencePattern;

        if (Schedule.RecurrencePattern == RecurrencePattern.None)
        {
            Schedule.AllDay = stdAllDay.Checked;

            Schedule.NoEndDateTime = stdNoEndTime.Checked;
            if (Schedule.NoEndDateTime)
            {
                Schedule.EndDateTime = base.ToUTC(stdStartDate.SelectedDate.AddDays(1));
            }

            if (Schedule.AllDay)
            {
                Schedule.StartDateTime = base.ToUTC(stdStartDate.SelectedDate);
                Schedule.EndDateTime = base.ToUTC(stdEndDate.SelectedDate);
            }
            else
            {
                Schedule.StartDateTime = base.ToUTC(stdStartDate.SelectedDate.Add(stdStartTime.SelectedTime));
                Schedule.EndDateTime = base.ToUTC(stdEndDate.SelectedDate.Add(stdEndTime.SelectedTime));
            }
        }
        else
        {
            Schedule.StartDateTime = base.ToUTC(recStartDate.SelectedDate.Add(recStartTime.SelectedTime));

            Schedule.NoEndDateTime = stdNoEndTime.Checked;
            if (Schedule.NoEndDateTime)
            {
                Schedule.EndDateTime = base.ToUTC(stdStartDate.SelectedDate.AddDays(1));
            }
            else
            {
                Schedule.EndDateTime = base.ToUTC(recEndByDate.SelectedDate.Add(recEndTime.SelectedTime));
            }

            Schedule.Endless = recNoEndDate.Checked;
            Schedule.EndOccurrences = recEndAfter.Checked ? int.Parse(recEndAfterNOccurences.Text) : 0;
            switch (Schedule.RecurrencePattern)
            {
                case RecurrencePattern.Daily_EveryNDays:
                    Schedule.DailyEveryNDays = int.Parse(recDailyEveryNDays.Text);
                    break;
                case RecurrencePattern.Daily_EveryWeekday:
                    break;
                case RecurrencePattern.Weekly:
                    Schedule.WeeklyDaysOfWeek = 0;
                    for (int i = 0; i < 7; i++)
                        if (recWeeklyDaysOfWeek.Items[i].Selected)
                            Schedule.WeeklyDaysOfWeek += (short)Math.Pow(2, i);
                    Schedule.WeeklyEveryNWeeks = int.Parse(recWeeklyEveryNWeeks.Text);
                    break;
                case RecurrencePattern.Monthly_DayNOfEveryNMonths:
                    Schedule.MonthlyDay = int.Parse(recMonthlyDay.Text);
                    Schedule.MonthlyMonth = int.Parse(recMonthlyMonth.Text);
                    break;
                case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
                    Schedule.MonthlyExDayIndex = int.Parse(recMonthlyDayIndex.SelectedValue);
                    Schedule.MonthlyExDayName = int.Parse(recMonthlyDayName.SelectedValue);
                    Schedule.MonthlyExMonth = int.Parse(recMonthlyExMonth.Text);
                    break;
                case RecurrencePattern.Yearly_DayNOfMonth:
                    Schedule.YearlyDay = int.Parse(recYearlyDay.Text);
                    Schedule.YearlyMonth = int.Parse(recYearlyMonth.Text);
                    break;
                case RecurrencePattern.Yearly_NthWeekDayOfMonth:
                    Schedule.YearlyExDayIndex = int.Parse(recYearlyExDayIndex.Text);
                    Schedule.YearlyExDayName = int.Parse(recYearlyExDayName.Text);
                    Schedule.YearlyExMonth = int.Parse(recYearlyExMonth.Text);
                    break;
            }

        }
    }

    public override string ToString()
    {
        return SessionManager.ObjectService.GetScheduleString(
            SessionManager.Ticket, Schedule, SessionManager.UtcOffset);
    }

    public void stdAllDay_CheckedChanged(object sender, EventArgs e)
    {
        Schedule.AllDay = !Schedule.AllDay;
    }

    public void stdNoEndTime_CheckedChanged(object sender, EventArgs e)
    {
        Schedule.NoEndDateTime = !Schedule.NoEndDateTime;
        recNoEndTime.Checked = Schedule.NoEndDateTime;
    }

    public void recNoEndTime_CheckedChanged(object sender, EventArgs e)
    {
        Schedule.NoEndDateTime = !Schedule.NoEndDateTime;
        stdNoEndTime.Checked = Schedule.NoEndDateTime;
    }

    protected override void OnPreRender(EventArgs e)
    {
        stdStartTime.Visible = !Schedule.AllDay;
        
        stdEndTime.Visible = !Schedule.AllDay && !Schedule.NoEndDateTime;
        labelRecEndTime.Visible = labelEndTime.Visible = stdEndDate.Visible = !Schedule.NoEndDateTime;
        
        recEndTime.Visible = !Schedule.NoEndDateTime;

        HighLevelRecurrencePattern shlrp = SelectedHighLevelRecurrencePattern;
        recDaily.Visible = (shlrp == HighLevelRecurrencePattern.Daily);
        recWeekly.Visible = (shlrp == HighLevelRecurrencePattern.Weekly);
        recMonthly.Visible = (shlrp == HighLevelRecurrencePattern.Monthly);
        recYearly.Visible = (shlrp == HighLevelRecurrencePattern.Yearly);

        base.OnPreRender(e);
    }

    public void editCurrent_Click(object sender, EventArgs e)
    {
        if (Schedule.RecurrencePattern == RecurrencePattern.None)
        {
            addOneTime_Click(sender, e);
        }
        else
        {
            addRecurrent_Click(sender, e);
        }
    }

    public void addOneTime_Click(object sender, EventArgs e)
    {
        addRecurrent.Enabled = true;
        addOneTime.Enabled = false;
        editCurrent.Enabled = false;
        Schedule.RecurrencePattern = RecurrencePattern.None;
        panelSchedule.Visible = true;
        panelStandard.Visible = true;
        panelRecurrent.Visible = false;
        panelConfirmed.Visible = false;
        IsConfirmed = false;
    }

    public void addRecurrent_Click(object sender, EventArgs e)
    {
        addRecurrent.Enabled = false;
        addOneTime.Enabled = true;
        editCurrent.Enabled = false;
        Schedule.RecurrencePattern = RecurrencePattern.Daily_EveryNDays;
        UpdateSelection();
        panelSchedule.Visible = true;
        panelStandard.Visible = false;
        panelRecurrent.Visible = true;
        panelConfirmed.Visible = false;
        IsConfirmed = false;
    }

    public void ConfirmSchedule(object sender, EventArgs e)
    {
        if (IsConfirmed)
            return;

        UpdateSchedule();
        VerifySchedule();
        labelConfirmed.Text = this.ToString();
        addRecurrent.Enabled = true;
        addOneTime.Enabled = true;
        editCurrent.Enabled = true;
        panelConfirmed.Visible = true;
        panelSchedule.Visible = false;
        IsConfirmed = true;
        if (Confirm != null) Confirm(sender, e);
        panelSelectSchedule.Update();
        panelConfirmedUpdate.Update();
    }
}
