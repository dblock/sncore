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
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountPlacesManage : AuthenticatedPage
{
    private TransitPlaceQueryOptions mOptions = null;

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public TransitPlaceQueryOptions QueryOptions
    {
        get
        {
            if (mOptions == null)
            {
                mOptions = new TransitPlaceQueryOptions();
                mOptions.AccountId = SessionManager.Account.Id;
            }
            return mOptions;
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.PlaceService.GetPlacesCount(
            SessionManager.Ticket, QueryOptions);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.PlaceService.GetPlaces(
            SessionManager.Ticket, QueryOptions, options);
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:

                int id = int.Parse(e.CommandArgument.ToString());
                switch (e.CommandName)
                {
                    case "Delete":
                        SessionManager.PlaceService.DeletePlace(SessionManager.Ticket, id);
                        ReportInfo("Place deleted.");
                        GetData(source, e);
                        break;
                }
                break;
        }
    }
}
