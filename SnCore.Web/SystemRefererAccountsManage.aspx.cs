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
using Wilco.Web.UI.WebControls;
using System.Drawing;
using SnCore.Tools.Drawing;
using System.IO;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;

public partial class SystemRefererAccountsManage : AuthenticatedPage
{
    public void Page_Load()
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                gridManage.VirtualItemCount = StatsService.GetRefererAccountsCount();
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Referer Accounts", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = StatsService.GetRefererAccounts(options);
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
                            StatsService.DeleteRefererAccount(SessionManager.Ticket, id);
                            ReportInfo("Referer account deleted.");
                            gridManage.CurrentPageIndex = 0;
                            gridManage_OnGetDataSource(source, e);
                            gridManage.DataBind();
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
}
