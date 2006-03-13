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
    private object mSelectedDateTime = null;

    protected override void OnInit(EventArgs e)
    {        
        selectdateYear.Items.Add(new ListItem());
        for (int i = 1905; i <= DateTime.Now.Year; i++)
        {
            selectdateYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

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

        selectdateMonth.Items.Add(new ListItem());
        for (int i = 0; i < months.Length; i++)
        {
            selectdateMonth.Items.Add(new ListItem(months[i], (i + 1).ToString()));
        }

        base.OnInit(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (!IsPostBack && mSelectedDateTime != null)
        {
            DateTime d = (DateTime) mSelectedDateTime;
            selectdateMonth.Items.FindByValue(d.Month.ToString()).Selected = true;
            selectdateDay.Items.FindByValue(d.Day.ToString()).Selected = true;
            selectdateYear.Items.FindByValue(d.Year.ToString()).Selected = true;
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
        }
    }
}
