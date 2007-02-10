using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class MarketingCampaignAccountRecepientsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            TransitCampaign tc = SessionManager.MarketingService.GetCampaignById(SessionManager.Ticket, RequestId);
            campaignName.Text = string.Format("{0}: {1}", Render(tc.Name), campaignName.Text);
            GetData(sender, e);

            inputAccountPropertyGroup.DataSource = SessionManager.AccountService.GetAccountPropertyGroups(
                SessionManager.Ticket, null);
            inputAccountPropertyGroup.DataBind();
            inputAccountPropertyGroup_SelectedIndexChanged(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Marketing Campaigns", Request, "MarketingCampaignsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(tc.Name, Request, string.Format("MarketingCampaignEdit.aspx?id={0}", tc.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Recepients", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.MarketingService.GetCampaignAccountRecepientsCount(
            SessionManager.Ticket, RequestId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.MarketingService.GetCampaignAccountRecepients(
            SessionManager.Ticket, RequestId, options);
    }

    public void importSingleUser_Click(object sender, EventArgs e)
    {
        List<TransitCampaignAccountRecepient> recepients = new List<TransitCampaignAccountRecepient>();
        foreach (string sid in importSingleUserIds.Text.Split(" ".ToCharArray()))
        {
            TransitCampaignAccountRecepient recepient = new TransitCampaignAccountRecepient();
            recepient.AccountId = int.Parse(sid);
            recepient.CampaignId = RequestId;
            recepient.Sent = false;
            recepients.Add(recepient);
        }

        int count = SessionManager.MarketingService.ImportCampaignAccountRecepients(SessionManager.Ticket, recepients.ToArray());
        GetData(sender, e);
        ReportInfo(string.Format("Successfully imported {0} recepients.", count));
    }

    private enum Cells
    {
        id = 0
    };

    public void importAllUsers_Click(object sender, EventArgs e)
    {
        int count = SessionManager.MarketingService.ImportCampaignAccountEmails(SessionManager.Ticket, RequestId,
            importAllVerifiedEmails.Checked, importAllUnverifiedEmails.Checked);
        GetData(sender, e);
        ReportInfo(string.Format("Successfully imported {0} recepients.", count));
    }

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                switch (e.CommandName)
                {
                    case "Delete":
                        SessionManager.Delete<TransitCampaignAccountRecepient>(id, SessionManager.MarketingService.DeleteCampaignAccountRecepient);
                        ReportInfo("Campaign account recepient deleted.");
                        GetData(source, e);
                        break;
                }
                break;
        }
    }

    public void deleteAllRecepients_Click(object sender, EventArgs e)
    {
        SessionManager.Delete<TransitCampaignAccountRecepient>(RequestId, SessionManager.MarketingService.DeleteCampaignAccountRecepients);
        GetData(sender, e);
    }

    public void inputAccountPropertyGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        inputAccountProperty.DataSource = SessionManager.AccountService.GetAccountProperties(
            SessionManager.Ticket, int.Parse(inputAccountPropertyGroup.SelectedValue), null);
        inputAccountProperty.DataBind();
    }

    public void importAccountProperty_Click(object sender, EventArgs e)
    {
        int count = SessionManager.MarketingService.ImportCampaignAccountPropertyValues(SessionManager.Ticket, RequestId,
            int.Parse(inputAccountProperty.SelectedValue), inputAccountPropertyValue.Text, inputAccountPropertyEmpty.Checked);
        GetData(sender, e);
        ReportInfo(string.Format("Successfully imported {0} recepients.", count));
    }
}
