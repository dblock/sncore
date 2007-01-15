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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;

public partial class SystemCountriesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Countries", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.LocationService.GetCountriesCount(
            SessionManager.Ticket);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.LocationService.GetCountries(
            SessionManager.Ticket, options);
    }

    private enum Cells
    {
        id = 0
    };

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
                        SessionManager.LocationService.DeleteCountry(SessionManager.Ticket, id);
                        ReportInfo("Country deleted.");
                        gridManage.CurrentPageIndex = 0;
                        gridManage_OnGetDataSource(source, e);
                        gridManage.DataBind();
                        break;
                }
                break;
        }
    }
}
