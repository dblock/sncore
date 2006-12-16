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

[SiteMapDataAttribute("About")]
public partial class About : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private TransitStatsSummary mSummary = null;

    public TransitStatsSummary Summary
    {
        get
        {
            if (mSummary == null)
            {
                mSummary = SessionManager.GetCachedItem<TransitStatsSummary>(
                    SessionManager.StatsService, "GetSummary", null);
            }
            return mSummary;
        }
    }

    public string Copyright
    {
        get
        {
            return SessionManager.SystemService.GetCopyright();
        }
    }

    public string Version
    {
        get
        {
            return SessionManager.SystemService.GetVersion();
        }
    }

    public TimeSpan WebServicesUptime
    {
        get
        {
            return new TimeSpan(SessionManager.SystemService.GetUptime());
        }
    }

    public TimeSpan BackEndServicesUptime
    {
        get
        {
            return new TimeSpan(SessionManager.BackEndService.GetUptime());
        }
    }

    public TimeSpan Uptime
    {
        get
        {
            return DateTime.UtcNow - Global.Started;
        }
    }
}
