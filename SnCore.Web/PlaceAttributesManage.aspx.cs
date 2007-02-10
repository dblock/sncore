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

public partial class PlaceAttributesManage : AuthenticatedPage
{
    private TransitPlace mPlace = null;
    public TransitPlace Place
    {
        get
        {
            if (mPlace == null)
            {
                mPlace = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, RequestId);
            }

            return mPlace;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        TransitPlace p = Place;

        linkNew.NavigateUrl = string.Format("PlaceAttributeEdit.aspx?aid={0}", RequestId);
        placeLink.HRef = string.Format("PlaceView.aspx?id={0}", RequestId);
        placeImage.Src = string.Format("PlacePictureThumbnail.aspx?id={0}", p.PictureId);
        placeName.Text = Render(p.Name);

        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("PlaceView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Attributes", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.PlaceService.GetPlaceAttributesCount(
            SessionManager.Ticket, RequestId);
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
        gridManage.DataSource = SessionManager.PlaceService.GetPlaceAttributes(
            SessionManager.Ticket, RequestId, options);
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                SessionManager.Delete<TransitPlaceAttribute>(id, SessionManager.PlaceService.DeletePlaceAttribute);
                ReportInfo("Place attribute deleted.");
                gridManage.CurrentPageIndex = 0;
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();
                break;
        }
    }
}
