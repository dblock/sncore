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
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                GetData(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Countries", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = LocationService.GetCountriesCount();
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = LocationService.GetCountries(options);
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
                            LocationService.DeleteCountry(SessionManager.Ticket, id);
                            ReportInfo("Country deleted.");
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
