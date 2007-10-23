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
using SnCore.SiteMap;
using SnCore.Services;

public partial class SystemAccountQuotasManage : AuthenticatedPage
{
    private TransitAccount mAccount = null;

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = SessionManager.AccountService.GetAccountById(
                    SessionManager.Ticket, RequestId);
            }

            return mAccount;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetObjectsData(sender, e);
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(Account.Name, Request,
                string.Format("AccountView.aspx?id={0}", RequestId)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Quotas", Request.Url));
            StackSiteMap(sitemapdata);           
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountQuota, int>(
            RequestId, SessionManager.AccountService.GetAccountQuotasCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private enum Cells
    {
        id = 0
    };

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountQuota, int>(
           RequestId, options, SessionManager.AccountService.GetAccountQuotas);
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                SessionManager.Delete<TransitAccountQuota>(id, SessionManager.AccountService.DeleteAccountQuota);
                ReportInfo("Quota deleted.");
                GetData(sender, e);
                break;
        }
    }

    public void GetObjectsData(object sender, EventArgs e)
    {
        IList<TransitDataObject> objects = SessionManager.GetCollection<TransitDataObject>(
            null, SessionManager.ObjectService.GetDataObjects);
        objects.Insert(0, new TransitDataObject());
        inputObject.DataSource = objects;
        inputObject.DataBind();
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountQuota t_instance = null;
        try
        {
            t_instance = SessionManager.AccountService.GetAccountQuotaByObjectName(
                SessionManager.Ticket, RequestId, inputObject.SelectedValue);
        }
        catch
        {
            t_instance = new TransitAccountQuota();
        }

        t_instance.Limit = int.Parse(inputLimit.SelectedValue);
        t_instance.DataObjectName = inputObject.SelectedValue;
        t_instance.AccountId = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountQuota>(t_instance, 
            SessionManager.AccountService.CreateOrUpdateAccountQuota);
        GetData(sender, e);
    }
}
