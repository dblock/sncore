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

public partial class SelectTimeControl : System.Web.UI.UserControl
{
    public event EventHandler SelectionChanged;

    private object mSelectedTime = null;

    internal string ToString(TimeSpan ts)
    {
        return string.Format("{0}:{1} {2}",
            (ts.Hours > 12 ? ts.Hours - 12 : ts.Hours).ToString(),
            ts.Minutes.ToString("00"),
            (ts.Hours >= 12) ? "PM" : "AM"); 
    }

    protected override void OnInit(EventArgs e)
    {
        TimeSpan ts = new TimeSpan(0);

        do
        {
            selecttimeDropdown.Items.Add(new ListItem(ToString(ts), ts.Ticks.ToString()));
            ts = ts.Add(new TimeSpan(0, 15, 0));
        } while (ts.Hours != 0 || ts.Minutes != 0);

        ts = new TimeSpan(11, 59, 0);
        selecttimeDropdown.Items.Add(new ListItem(ToString(ts), ts.Ticks.ToString()));

        base.OnInit(e);
    }

    internal void SelectTime()
    {
        selecttimeDropdown.ClearSelection();

        if (mSelectedTime == null)
            return;

        TimeSpan ts = (TimeSpan)mSelectedTime;

        ListItem item = selecttimeDropdown.Items.FindByValue(ts.Ticks.ToString());
        
        if (item == null)
            return;

        item.Selected = true;
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (!IsPostBack)
        {
            SelectTime();
        }
        base.OnPreRender(e);
    }

    public bool HasTime
    {
        get
        {
            return ! string.IsNullOrEmpty(selecttimeDropdown.SelectedValue);
        }
    }

    public DateTime SelectedTimeToday
    {
        get
        {
            DateTime result = DateTime.UtcNow;
            result = result.Subtract(result.TimeOfDay);
            result = result.Add(SelectedTime);
            return result;
        }
    }

    public TimeSpan SelectedTime
    {
        get
        {
            mSelectedTime = new TimeSpan(long.Parse(selecttimeDropdown.SelectedValue));
            return (TimeSpan)mSelectedTime;
        }
        set
        {
            TimeSpan nv = new TimeSpan(value.Hours, value.Minutes, 0);

            if ((value.Hours != 11) || (value.Minutes != 59))
            {
                int rem = 0;
                Math.DivRem(nv.Minutes, 15, out rem);
                while (rem != 0)
                {
                    nv = nv.Subtract(new TimeSpan(0, 1, 0));
                    Math.DivRem(nv.Minutes, 15, out rem);
                };
            }

            mSelectedTime = nv;
            SelectTime();
        }
    }

    public void selectionChanged(object s, EventArgs e)
    {
        if (SelectionChanged != null) SelectionChanged(s, e);
    }
}
