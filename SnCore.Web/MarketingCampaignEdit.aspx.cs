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
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class MarketingCampaignEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Marketing Campaigns", Request, "MarketingCampaignsManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("Campaign");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputSenderName.MaxLength = cs["SenderName"].MaxLengthInChars;
            inputSenderEmail.MaxLength = cs["SenderEmailAddress"].MaxLengthInChars;
            inputUrl.MaxLength = cs["Url"].MaxLengthInChars;

            if (RequestId > 0)
            {
                TransitCampaign t = SessionManager.MarketingService.GetCampaignById(SessionManager.Ticket, RequestId);
                inputActive.Checked = t.Active;
                inputDescription.Text = t.Description;
                inputName.Text = t.Name;
                inputSenderEmail.Text = t.SenderEmailAddress;
                inputSenderName.Text = t.SenderName;
                inputUrl.Text = t.Url;
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Marketing Campaign", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(save);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitCampaign t = new TransitCampaign();
        t.Id = RequestId;
        t.Active = inputActive.Checked;
        t.Description = inputDescription.Text;
        t.Name = inputName.Text;
        t.SenderEmailAddress = inputSenderEmail.Text;
        t.SenderName = inputSenderName.Text;
        t.Url = inputUrl.Text;
        SessionManager.CreateOrUpdate<TransitCampaign>(
            t, SessionManager.MarketingService.CreateOrUpdateCampaign);
        Redirect("MarketingCampaignsManage.aspx");
    }
}
