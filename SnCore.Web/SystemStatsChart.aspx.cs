using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using WebChart;
using System.Collections.Generic;
using SnCore.Services;
using System.Drawing;
using SnCore.Tools.Drawing;

public partial class SystemStatsChart2 : PicturePage
{
    private TransitStatsSummary mSummary = null;

    public TransitStatsSummary Summary
    {
        get
        {
            if (mSummary == null)
            {
                mSummary = SessionManager.GetInstance<TransitStatsSummary>(
                    SessionManager.StatsService.GetSummary);
            }

            return mSummary;
        }
    }

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

    public ChartType RequestType
    {
        get
        {
            return (ChartType)Enum.Parse(typeof(ChartType), Request.Params["type"]);
        }
    }

    public override PicturePage.PicturePageType PageType
    {
        get
        {
            return PicturePageType.Bitmap;
        }
    }

    public override TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince)
    {
        return GetPictureWithBitmap(id);
    }

    public override TransitPicture GetPictureWithThumbnail(int id, DateTime ifModifiedSince)
    {
        if (ifModifiedSince.AddMinutes(1) >= DateTime.UtcNow)
            return null;

        throw new NotImplementedException();
    }

    public override TransitPicture GetPictureWithBitmap(int id)
    {
        ChartEngine engine = new ChartEngine();
        engine.Size = new Size(570, 300);
        engine.GridLines = WebChart.GridLines.None;
        engine.ShowXValues = true;
        engine.ShowYValues = true;
        engine.LeftChartPadding = 50;
        engine.BottomChartPadding = 50;
        engine.XAxisFont.StringFormat.LineAlignment = StringAlignment.Center;
        engine.XAxisFont.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
        engine.XAxisFont.ForeColor = engine.YAxisFont.ForeColor = Color.Black;

        ChartCollection charts = new ChartCollection(engine);
        engine.Charts = charts;

        List<List<TransitSummarizedCounter>> counters = new List<List<TransitSummarizedCounter>>();
        string format;

        switch (RequestType)
        {
            case ChartType.DailyNew:
                counters.Add(Summary.NewDaily);
                format = "MMM d";
                break;
            case ChartType.DailyReturning:
                counters.Add(Summary.ReturningDaily);
                format = "MMM d";
                break;
            case ChartType.Daily:
                counters.Add(Summary.Daily);
                format = "MMM d";
                break;
            case ChartType.Hourly:
                counters.Add(Summary.Hourly);
                format = "htt";
                break;
            case ChartType.MonthlyUnique:
                counters.Add(Summary.UniqueMonthly);
                format = "MMM";
                break;
            case ChartType.Monthly:
                counters.Add(Summary.Monthly);
                format = "MMM";
                break;
            case ChartType.Weekly:
                counters.Add(Summary.Weekly);
                format = "MMM dd";
                break;
            case ChartType.Yearly:
                counters.Add(Summary.Yearly);
                format = "yyyy";
                break;
            case ChartType.AccountDaily:
                counters.Add(Summary.AccountDaily);
                format = "MMM d";
                break;
            case ChartType.AccountMonthly:
                counters.Add(Summary.AccountMonthly);
                format = "MMM";
                break;
            case ChartType.AccountWeekly:
                counters.Add(Summary.AccountWeekly);
                format = "MMM dd";
                break;
            case ChartType.AccountYearly:
                counters.Add(Summary.AccountYearly);
                format = "yyyy";
                break;
            default:
                throw new ArgumentOutOfRangeException("type");
        }

        Color fill = Color.FromArgb(0x9F, 0x6, 0x15);
        foreach (List<TransitSummarizedCounter> clist in counters)
        {
            ColumnChart chart = new ColumnChart();
            chart.ShowLineMarkers = false;
            chart.ShowLegend = true;
            chart.Line.Color = Color.White;
            chart.Line.Width = 2;
            chart.Fill.Color = engine.Border.Color = fill;
            chart.Fill.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            chart.MaxColumnWidth = 100;

            foreach (TransitSummarizedCounter counter in clist)
            {
                chart.Data.Add(new ChartPoint(counter.Timestamp.ToString(format), counter.Total));
            }

            charts.Add(chart);
            fill = Color.FromArgb(fill.R + 0x30, fill.G + 0x30, fill.B + 0x30);
        }

        TransitPicture picture = new TransitPicture();
        picture.Id = 0;
        picture.Modified = picture.Created = DateTime.UtcNow;
        picture.Name = RequestType.ToString();

        MemoryStream ds = new MemoryStream();
        engine.GetBitmap().Save(ds, System.Drawing.Imaging.ImageFormat.Png);
        picture.Bitmap = new byte[ds.Length];
        MemoryStream ms = new MemoryStream(picture.Bitmap);
        ds.WriteTo(ms);

        return picture;
    }

    public override TransitPicture GetPictureWithThumbnail(int id)
    {
        throw new NotImplementedException();
    }

    public override TransitPicture GetRandomPictureWithThumbnail()
    {
        throw new NotImplementedException();
    }
}
