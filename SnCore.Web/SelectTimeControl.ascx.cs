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
    private object mSelectedTime = null;

    protected override void OnInit(EventArgs e)
    {
        TimeSpan ts = new TimeSpan(0);

        do
        {
            selecttimeDropdown.Items.Add(new ListItem(string.Format("{0}:{1}", ts.Hours.ToString("00"), ts.Minutes.ToString("00"))));
            ts = ts.Add(new TimeSpan(0, 15, 0));
        } while (ts.Hours != 0 || ts.Minutes != 0);

        base.OnInit(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (!IsPostBack && mSelectedTime != null)
        {
            selecttimeDropdown.Items.FindByValue(string.Format("{0}:{1}",
                ((TimeSpan)mSelectedTime).Hours.ToString("00"), ((TimeSpan)mSelectedTime).Minutes.ToString("00"))).Selected = true;
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
            string[] tm = selecttimeDropdown.SelectedValue.Split(":".ToCharArray());
            mSelectedTime = new TimeSpan(
                int.Parse(tm[0]),
                int.Parse(tm[1]),
                0);

            return (TimeSpan)mSelectedTime;
        }
        set
        {
            if (value.Seconds != 0)
                value = value.Subtract(new TimeSpan(0, 0, value.Seconds));

            int rem = 0;
            Math.DivRem(value.Minutes, 15, out rem);
            while (rem != 0)
            {
                value = value.Subtract(new TimeSpan(0, 1, 0));
                Math.DivRem(value.Minutes, 15, out rem);
            };

            mSelectedTime = value;

            selecttimeDropdown.ClearSelection();

            selecttimeDropdown.Items.FindByValue(string.Format("{0}:{1}",
                ((TimeSpan)mSelectedTime).Hours.ToString("00"), ((TimeSpan)mSelectedTime).Minutes.ToString("00"))).Selected = true;        
        }
    }
}
