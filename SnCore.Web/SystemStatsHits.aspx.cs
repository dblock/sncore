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
using SnCore.SiteMap;

[SiteMapDataAttribute("Statistics")]
public partial class SystemStatsHits : AuthenticatedPage
{
    public enum ChartType
    {
        Hourly,
        Daily,
        DailyNew,
        DailyReturning,
        Weekly,
        Monthly,
        MonthlyUnique,
        Yearly,
        AccountDaily,
        AccountWeekly,
        AccountMonthly,
        AccountYearly
    }

    public void Page_Load()
    {
        if (!SessionManager.IsAdministrator)
        {
            ReportWarning("This page is only available to the system administrator.");
            imageStats.Visible = false;
            return;
        }

        if (!IsPostBack)
        {
            SetChartType(ChartType.Weekly);
        }
    }

    void SetChartType(ChartType type)
    {
        imageStats.Src = string.Format("SystemStatsChart.aspx?type={0}&CacheDuration=300", type);
        switch (type)
        {
            case ChartType.DailyNew:
                labelChartType.Text = "New Visitors";
                break;
            case ChartType.DailyReturning:
                labelChartType.Text = "Returning Visitors";
                break;
            case ChartType.MonthlyUnique:
                labelChartType.Text = "Monthly Unique";
                break;
            case ChartType.AccountDaily:
            case ChartType.AccountMonthly:
            case ChartType.AccountWeekly:
            case ChartType.AccountYearly:
                labelChartType.Text = type.ToString().Replace("Account", "New Accounts ");
                break;
            default:
                labelChartType.Text = type.ToString();
                break;
        }

        linkDaily.Enabled = (type != ChartType.Daily);
        linkHourly.Enabled = (type != ChartType.Hourly);
        linkMonthly.Enabled = (type != ChartType.Monthly);
        linkYearly.Enabled = (type != ChartType.Yearly);
        linkWeekly.Enabled = (type != ChartType.Weekly);

        linkAccountDaily.Enabled = (type != ChartType.AccountDaily);
        linkAccountMonthly.Enabled = (type != ChartType.AccountMonthly);
        linkAccountYearly.Enabled = (type != ChartType.AccountYearly);
        linkAccountWeekly.Enabled = (type != ChartType.AccountWeekly);

        linkDailyNew.Enabled = (type != ChartType.DailyNew);
        linkDailyReturning.Enabled = (type != ChartType.DailyReturning);

        linkMonthlyUnique.Enabled = (type != ChartType.MonthlyUnique);
    }

    public void linkDailyNew_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.DailyNew);
    }

    public void linkMonthlyUnique_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.MonthlyUnique);
    }

    public void linkDailyReturning_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.DailyReturning);
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

    public void linkAccountYearly_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.AccountYearly);
    }

    public void linkAccountMonthly_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.AccountMonthly);
    }

    public void linkAccountWeekly_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.AccountWeekly);
    }

    public void linkAccountDaily_Click(object sender, EventArgs e)
    {
        SetChartType(ChartType.AccountDaily);
    }

}
