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

public partial class SelectDateControl : System.Web.UI.UserControl
{
    public event EventHandler SelectionChanged;

    private object mSelectedDateTime = null;
    private bool mRequiresSelection = false;
    private int mFutureYears = 0;
    private int mPastYears = 120;

    public int FutureYears
    {
        get
        {
            return mFutureYears;
        }
        set
        {
            mFutureYears = value;
        }
    }

    public int PastYears
    {
        get
        {
            return mPastYears;
        }
        set
        {
            mPastYears = value;
        }
    }

    public bool RequiresSelection
    {
        get
        {
            return mRequiresSelection;
        }
        set
        {
            mRequiresSelection = value;
        }
    }

    public void selectDateCalendar_SelectionChanged(object s, EventArgs e)
    {
        SelectedDate = selectDateCalendar.SelectedDate;
        if (SelectionChanged != null) SelectionChanged(s, e);
    }

    public bool HasSelection
    {
        get
        {
            int tmpresult;
            return
                int.TryParse(selectdateYear.SelectedValue, out tmpresult) &&
                int.TryParse(selectdateMonth.SelectedValue, out tmpresult) &&
                int.TryParse(selectdateDay.SelectedValue, out tmpresult);
        }
    }

    protected override void OnInit(EventArgs e)
    {
        if (! RequiresSelection)
            selectdateYear.Items.Add(new ListItem());

        for (int i = DateTime.UtcNow.Year + FutureYears; i >= DateTime.UtcNow.Year - PastYears; i--)
        {
            selectdateYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        if (! RequiresSelection)
            selectdateDay.Items.Add(new ListItem());
        for (int i = 1; i <= 31; i++)
        {
            selectdateDay.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        string[] months = {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };


        if (! RequiresSelection)
            selectdateMonth.Items.Add(new ListItem());
        for (int i = 0; i < months.Length; i++)
        {
            selectdateMonth.Items.Add(new ListItem(months[i], (i + 1).ToString()));
        }

        base.OnInit(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (SelectionChanged == null)
        {
            selectdateDay.AutoPostBack = false;
            selectdateMonth.AutoPostBack = false;
            selectdateYear.AutoPostBack = false;
        }

        if (!IsPostBack && mSelectedDateTime != null)
        {
            DateTime d = (DateTime) mSelectedDateTime;
            selectdateMonth.ClearSelection();
            selectdateMonth.Items.FindByValue(d.Month.ToString()).Selected = true;
            selectdateDay.ClearSelection();
            selectdateDay.Items.FindByValue(d.Day.ToString()).Selected = true;

            ListItem selectedYearItem = selectdateYear.Items.FindByValue(d.Year.ToString());
            if (selectedYearItem == null)
            {
                selectedYearItem = new ListItem(d.Year.ToString(), d.Year.ToString());
                selectdateYear.Items.Add(selectedYearItem);
            }

            selectedYearItem.Selected = true;

            if (HasSelection)
            {
                selectDateCalendar.SelectedDate = SelectedDate;
                selectDateCalendar.VisibleDate = SelectedDate;
            }
        }
        base.OnPreRender(e);
    }

    public bool HasDate
    {
        get
        {
            return (!string.IsNullOrEmpty(selectdateYear.SelectedValue)
                    && !string.IsNullOrEmpty(selectdateMonth.SelectedValue)
                    && !string.IsNullOrEmpty(selectdateDay.SelectedValue));
        }
    }

    public DateTime SelectedDate
    {
        get
        {
            mSelectedDateTime = new DateTime(
                int.Parse(selectdateYear.SelectedValue),
                int.Parse(selectdateMonth.SelectedValue),
                int.Parse(selectdateDay.SelectedValue));

            return (DateTime)mSelectedDateTime;
        }
        set
        {
            mSelectedDateTime = value;

            selectdateMonth.ClearSelection();
            selectdateDay.ClearSelection();
            selectdateYear.ClearSelection();

            DateTime d = (DateTime) mSelectedDateTime;
            Control.SelectAndOrAddByValue(selectdateMonth.Items, d.Month.ToString());
            Control.SelectAndOrAddByValue(selectdateDay.Items, d.Day.ToString());
            Control.SelectAndOrAddByValue(selectdateYear.Items, d.Year.ToString());
        }
    }

    public void selectionChanged(object s, EventArgs e)
    {
        if (SelectionChanged != null) SelectionChanged(s, e);
    }
}
