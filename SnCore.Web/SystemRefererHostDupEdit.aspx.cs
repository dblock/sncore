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
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.Tools.Drawing;
using System.IO;
using SnCore.SiteMap;

public partial class SystemRefererHostDupEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Referer Host Dups", Request, "SystemRefererHostDupsManage.aspx"));

                if (RequestId > 0)
                {
                    TransitRefererHostDup t = SessionManager.StatsService.GetRefererHostDupById(RequestId);
                    inputHost.Text = t.Host;
                    inputRefererHost.Text = t.RefererHost;
                    sitemapdata.Add(new SiteMapDataAttributeNode(t.Host, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Referer Host", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(manageAdd);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitRefererHostDup t = new TransitRefererHostDup();
            t.Host = inputHost.Text;
            t.RefererHost = inputRefererHost.Text;
            t.Id = RequestId;
            SessionManager.StatsService.CreateOrUpdateRefererHostDup(SessionManager.Ticket, t);
            Redirect("SystemRefererHostDupsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
