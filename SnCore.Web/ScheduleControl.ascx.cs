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

            if (panelStandard.Visible)
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
                recWeeklyDaysOfWeek.Items.FindByValue(i.ToString()).Selected = true;
        }
    }

    protected void SelectMonthly()
    {
        recMonthlyDay.Text = Schedule.MonthlyDay.ToString();
        recMonthlyMonth.Text = Schedule.MonthlyMonth.ToString();

        recMonthlyDayIndex.ClearSelection();
        recMonthlyDayIndex.Items.FindByValue(Schedule.MonthlyExDayIndex.ToString()).Selected = true;

        recMonthlyDayName.ClearSelection();
        recMonthlyDayName.Items.FindByValue(Schedule.MonthlyExDayName.ToString()).Selected = true;

        recMonthlyExMonth.Text = Schedule.MonthlyExMonth.ToString();
    }

    protected void SelectYearly()
    {
        recYearlyMonth.ClearSelection();
        recYearlyMonth.Items.FindByValue(Schedule.YearlyMonth.ToString()).Selected = true;

        recYearlyDay.Text = Schedule.YearlyDay.ToString();

        recYearlyExDayName.ClearSelection();
        recYearlyExDayName.Items.FindByValue(Schedule.YearlyExDayName.ToString()).Selected = true;

        recYearlyExDayIndex.ClearSelection();
        recYearlyExDayIndex.Items.FindByValue(Schedule.YearlyExDayIndex.ToString()).Selected = true;

        recYearlyExMonth.ClearSelection();
        recYearlyExMonth.Items.FindByValue(Schedule.YearlyExMonth.ToString()).Selected = true;
    }

    protected void SelectRange()
    {
        recStartDate.SelectedDate = base.ToUTC(Schedule.StartDateTime);
        recEndByDate.SelectedDate = base.ToUTC(Schedule.EndDateTime);
        recEndAfterNOccurences.Text = Schedule.EndOccurrences.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageManager.SetDefaultButton(confirmSchedule, this.Controls);
        if (!IsPostBack)
        {
            stdStartDate.SelectedDate = base.Adjust(Schedule.StartDateTime);
            stdStartTime.SelectedTime = base.Adjust(Schedule.StartDateTime).TimeOfDay;

            stdEndDate.SelectedDate = base.Adjust(Schedule.EndDateTime);
            stdEndTime.SelectedTime = base.Adjust(Schedule.EndDateTime).TimeOfDay;

            stdAllDay.Checked = Schedule.AllDay;

            recStartTime.SelectedTime = base.Adjust(Schedule.StartDateTime).TimeOfDay;
            recEndTime.SelectedTime = base.Adjust(Schedule.EndDateTime).TimeOfDay;

            UpdateSelection();

            if (Schedule.Id != 0)
            {
                labelConfirmed.Text = this.ToString();
                addRecurrent.Enabled = false; // true;
                addOneTime.Enabled = true;
                editCurrent.Enabled = true;
                panelConfirmed.Visible = true;
                panelSchedule.Visible = false;
                IsConfirmed = true;
            }
        }
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
                else
                {
                    if (Schedule.EndDateTime <= Schedule.StartDateTime)
                    {
                        throw new Exception("End date/time must be after start date/time.");
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
            Schedule.EndDateTime = base.ToUTC(recEndByDate.SelectedDate.Add(recEndTime.SelectedTime));
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
        return SystemService.GetScheduleString(Schedule, SessionManager.UtcOffset);
    }

    public void stdAllDay_CheckedChanged(object sender, EventArgs e)
    {
        Schedule.AllDay = !Schedule.AllDay;
    }

    protected override void OnPreRender(EventArgs e)
    {
        stdEndTime.Visible = stdStartTime.Visible = !Schedule.AllDay;

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
        addRecurrent.Enabled = false; //  true;
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

    public void confirmSchedule_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateSchedule();
            VerifySchedule();
            labelConfirmed.Text = this.ToString();
            addRecurrent.Enabled = false; // true;
            addOneTime.Enabled = true;
            editCurrent.Enabled = true;
            panelConfirmed.Visible = true;
            panelSchedule.Visible = false;
            IsConfirmed = true;
            if (Confirm != null) Confirm(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void stdStart_SelectionChanged(object sender, EventArgs e)
    {
        DateTime projectedEndDateTime = stdStartDate.SelectedDate.Add(stdStartTime.SelectedTime).AddHours(1);
        DateTime currentEndDateTime = stdEndDate.SelectedDate.Add(stdEndTime.SelectedTime);
        if (currentEndDateTime < projectedEndDateTime)
        {
            stdEndDate.SelectedDate = projectedEndDateTime.Date;
            stdEndTime.SelectedTime = projectedEndDateTime.TimeOfDay;
        }
    }
}
