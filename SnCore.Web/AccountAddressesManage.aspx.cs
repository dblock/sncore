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
using SnCore.SiteMap;
using SnCore.Services;

public partial class AccountAddressesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Addresses", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.AccountService.GetAccountAddresses(
            SessionManager.Ticket, SessionManager.AccountId, null);
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
        switch (e.CommandName)
        {
            case "Delete":
                SessionManager.Delete<TransitAccountAddress>(id, SessionManager.AccountService.DeleteAccountAddress);
                ReportInfo("Address deleted.");
                gridManage.CurrentPageIndex = 0;
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();
                break;
        }
    }
}
