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
using System.Collections.Generic;
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class SystemAccountPlaceRequestsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "AccountPlacesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Property Requests", Request.Url));
                StackSiteMap(sitemapdata);
            }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
            List<TransitAccountPlaceRequest> requests;
            if (RequestId > 0)
            {
                requests = SessionManager.PlaceService.GetAccountPlaceRequestsById(SessionManager.Ticket, RequestId);
            }
            else
            {
                requests = SessionManager.PlaceService.GetAccountPlaceRequests(SessionManager.Ticket);
            }

            gridManage.DataSource = requests;

            if (requests == null || requests.Count == 0)
            {
                panelRequests.Visible = false;
                noticeRequests.Info = "No requests.";
            }
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
                            SessionManager.PlaceService.DeleteAccountPlaceRequest(SessionManager.Ticket, id);
                            ReportInfo("Request deleted.");
                            gridManage.CurrentPageIndex = 0;
                            gridManage_OnGetDataSource(source, e);
                            gridManage.DataBind();
                            break;
                        case "Accept":
                            SessionManager.PlaceService.AcceptAccountPlaceRequest(SessionManager.Ticket, id, inputMessage.Text);
                            ReportInfo("Request accepted.");
                            gridManage.CurrentPageIndex = 0;
                            gridManage_OnGetDataSource(source, e);
                            gridManage.DataBind();
                            break;
                        case "Reject":
                            SessionManager.PlaceService.RejectAccountPlaceRequest(SessionManager.Ticket, id, inputMessage.Text);
                            ReportInfo("Request rejected.");
                            gridManage.CurrentPageIndex = 0;
                            gridManage_OnGetDataSource(source, e);
                            gridManage.DataBind();
                            break;
                    }
                    break;
            }
    }

}
