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
using System.Collections.Generic;
using System.Web.Caching;
using System.Net;
using SnCore.SiteMap;

public partial class AccountLicenseEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkChooseLicense.NavigateUrl = string.Format("http://creativecommons.org/license/?partner={0}&exit_url={1}",
                Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")),
                Renderer.UrlEncode(Request.Url.ToString() + "?license_url=[license_url]&license_name=[license_name]&license_button=[license_button]"));

            string license_url = Request.QueryString["license_url"];
            string license_name = Request.QueryString["license_name"];
            string license_button = Request.QueryString["license_button"];

            TransitAccountLicense tal = SessionManager.LicenseService.GetAccountLicenseByAccountId(
                SessionManager.Ticket, SessionManager.AccountId);

            linkChooseLicense.Text = (tal == null) ? "&#187; Choose a License" : "&#187; Choose a New License";

            if (!string.IsNullOrEmpty(license_url))
            {
                if (tal == null)
                {
                    tal = new TransitAccountLicense();
                    tal.AccountId = SessionManager.Account.Id;
                }

                tal.LicenseUrl = Renderer.UrlDecode(license_url);
                tal.ImageUrl = Renderer.UrlDecode(license_button);
                tal.Name = Renderer.UrlDecode(license_name);

                tal.Id = SessionManager.LicenseService.CreateOrUpdateAccountLicense(SessionManager.Ticket, tal);

                Redirect("AccountLicenseEdit.aspx");
            }

            licenseImage.Visible = (tal != null);
            linkDeleteLicense.Visible = (tal != null);
            licenseDiv.Visible = (tal != null);

            if (tal != null && !string.IsNullOrEmpty(tal.LicenseUrl))
            {
                string key = string.Format("license:{0}", tal.Name);
                string license = (string)Cache[key];
                if (string.IsNullOrEmpty(license))
                {
                    WebClient client = new WebClient();
                    license = client.DownloadString(tal.LicenseUrl);
                    license = license.Substring(license.IndexOf("<div id=\"deed\""));
                    license = license.Substring(0, license.LastIndexOf("</div>") + 4);
                    license = Renderer.CleanHtml(license, new Uri("http://creativecommons.org/"), null);
                    Cache[key] = license;
                }

                licenseContent.Text = license;
                licenseImage.Src = tal.ImageUrl;
                licenseLink.HRef = tal.LicenseUrl;
                licenseImage.Alt = Renderer.Render(tal.Name);
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Creative License", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void linkDeleteLicense_Click(object sender, EventArgs e)
    {
        TransitAccountLicense tal = SessionManager.LicenseService.GetAccountLicenseByAccountId(
            SessionManager.Ticket, SessionManager.AccountId);
        if (tal != null) SessionManager.LicenseService.DeleteAccountLicense(SessionManager.Ticket, tal.Id);
        Redirect("AccountLicenseEdit.aspx");
    }
}
