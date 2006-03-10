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
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;
using System.Collections.Generic;

public partial class FeaturedAccountsRss : Page
{
    private string mWebsiteUrl = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ServiceQueryOptions queryoptions = new ServiceQueryOptions();
                queryoptions.PageNumber = 0;
                queryoptions.PageSize = 25;

                rssRepeater.DataSource = SystemService.GetFeatures("Account", queryoptions);
                rssRepeater.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string WebsiteUrl
    {
        get
        {
            if (string.IsNullOrEmpty(mWebsiteUrl))
            {
                mWebsiteUrl = SystemService.GetConfigurationByNameWithDefault(
                    "SnCore.WebSite.Url", "http://localhost/SnCoreWeb").Value;
            }

            return mWebsiteUrl;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }

    public string Link
    {
        get
        {
            return WebsiteUrl + "Default.aspx";
        }
    }

    public string GetSummary(string summary)
    {
        string result = Renderer.RemoveHtml(summary);
        if (result.Length > 256) result = result.Substring(0, 256) + " ...";
        return result;
    }

    public TransitAccount GetAccount(int id)
    {
        TransitAccount a = (TransitAccount)Cache[string.Format("account:{0}", id)];
        if (a == null)
        {
            a = AccountService.GetAccountById(id);
            Cache.Insert(string.Format("account:{0}", id),
                a, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
        }

        return a;
    }

    public string GetDescription(int id)
    {
        TransitAccountProfile a = (TransitAccountProfile)Cache[string.Format("accountprofile:{0}", id)];
        if (a == null)
        {
            List<TransitAccountProfile> aa = AccountService.GetAccountProfilesById(id);

            if (aa == null || aa.Count == 0)
                return string.Empty;

            a = aa[0];
            Cache.Insert(string.Format("accountprofile:{0}", id),
                a, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
        }

        return a.AboutSelf;
    }
}
