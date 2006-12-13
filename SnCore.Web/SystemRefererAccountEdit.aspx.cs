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

public partial class SystemRefererAccountEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Referer Accounts", Request, "SystemRefererAccountsManage.aspx"));

                if (RequestId > 0)
                {
                    TransitRefererAccount t = StatsService.GetRefererAccountById(RequestId);
                    inputAccount.Text = t.AccountId.ToString();
                    inputRefererHost.Text = t.RefererHostName;
                    sitemapdata.Add(new SiteMapDataAttributeNode(t.AccountName, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Referer Account", Request.Url));
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
            TransitRefererAccount t = new TransitRefererAccount();
            t.RefererHostName = inputRefererHost.Text;
            t.AccountId = int.Parse(inputAccount.Text);
            t.Id = RequestId;
            StatsService.CreateOrUpdateRefererAccount(SessionManager.Ticket, t);
            Redirect("SystemRefererAccountsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
