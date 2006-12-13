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
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                TransitCampaign tc = MarketingService.GetCampaignById(SessionManager.Ticket, RequestId);
                campaignName.Text = string.Format("{0}: {1}", Render(tc.Name), campaignName.Text);
                GetData(sender, e);

                inputAccountPropertyGroup.DataSource = AccountService.GetAccountPropertyGroups();
                inputAccountPropertyGroup.DataBind();
                inputAccountPropertyGroup_SelectedIndexChanged(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Marketing Campaigns", Request, "MarketingCampaignsManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(tc.Name, Request, string.Format("MarketingCampaignEdit.aspx?id={0}", tc.Id)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Recepients", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = MarketingService.GetCampaignAccountRecepientsByIdCount(SessionManager.Ticket, RequestId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = MarketingService.GetCampaignAccountRecepientsById(SessionManager.Ticket, RequestId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void importSingleUser_Click(object sender, EventArgs e)
    {
        try
        {
            List<TransitCampaignAccountRecepient> recepients = new List<TransitCampaignAccountRecepient>();
            foreach(string sid in importSingleUserIds.Text.Split(" ".ToCharArray()))
            {
                TransitCampaignAccountRecepient recepient = new TransitCampaignAccountRecepient();
                recepient.AccountId = int.Parse(sid);
                recepient.CampaignId = RequestId;
                recepient.Sent = false;
                recepients.Add(recepient);
            }

            int count = MarketingService.ImportCampaignAccountRecepients(SessionManager.Ticket, recepients.ToArray());
            GetData(sender, e);
            ReportInfo(string.Format("Successfully imported {0} recepients.", count));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    };

    public void importAllUsers_Click(object sender, EventArgs e)
    {
        try
        {
            int count = MarketingService.ImportCampaignAccountEmails(SessionManager.Ticket, RequestId, 
                importAllVerifiedEmails.Checked, importAllUnverifiedEmails.Checked);
            GetData(sender, e);
            ReportInfo(string.Format("Successfully imported {0} recepients.", count));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
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
                            MarketingService.DeleteCampaignAccountRecepient(SessionManager.Ticket, id);
                            ReportInfo("Campaign account recepient deleted.");
                            GetData(source, e);
                            break;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void deleteAllRecepients_Click(object sender, EventArgs e)
    {
        try
        {
            MarketingService.DeleteCampaignAccountRecepients(SessionManager.Ticket, RequestId);
            GetData(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void inputAccountPropertyGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            inputAccountProperty.DataSource = AccountService.GetAccountProperties(
                int.Parse(inputAccountPropertyGroup.SelectedValue));
            inputAccountProperty.DataBind();            
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void importAccountProperty_Click(object sender, EventArgs e)
    {
        try
        {
            int count = MarketingService.ImportCampaignAccountPropertyValues(SessionManager.Ticket, RequestId,
                int.Parse(inputAccountProperty.SelectedValue), inputAccountPropertyValue.Text, inputAccountPropertyEmpty.Checked);
            GetData(sender, e);
            ReportInfo(string.Format("Successfully imported {0} recepients.", count));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
