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

public partial class SystemStatsHits: AuthenticatedPage
{
    public enum ChartType
    {
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public void Page_Load()
    {
        if (!IsPostBack)
        {
            SetChartType(ChartType.Daily);
        }
    }

    void SetChartType(ChartType type)
    {
        imageStats.Src = string.Format("SystemStatsChart.aspx?type={0}", type);
        labelChartType.Text = type.ToString();

        linkDaily.Enabled = (type != ChartType.Daily);
        linkHourly.Enabled = (type != ChartType.Hourly);
        linkMonthly.Enabled = (type != ChartType.Monthly);
        linkYearly.Enabled = (type != ChartType.Yearly);
        linkWeekly.Enabled = (type != ChartType.Weekly);
    }

    public void linkYearly_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.Yearly);
    }

    public void linkHourly_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.Hourly);
    }

    public void linkMonthly_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.Monthly);
    }

    public void linkWeekly_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.Weekly);
    }

    public void linkDaily_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.Daily);
    }
}
