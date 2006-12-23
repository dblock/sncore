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
using SnCore.WebServices;
using SnCore.SiteMap;

public partial class SystemNeighborhoodsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                GetData(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Neighborhoods", Request.Url));
                StackSiteMap(sitemapdata);
            }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.LocationService.GetNeighborhoodsCount();
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = SessionManager.LocationService.GetNeighborhoods(options);
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
                            SessionManager.LocationService.DeleteNeighborhood(SessionManager.Ticket, id);
                            ReportInfo("Neighborhood deleted.");
                            GetData(source, e);
                            break;
                    }
                    break;
            }
    }
}