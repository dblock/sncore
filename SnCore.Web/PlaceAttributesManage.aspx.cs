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

public partial class PlaceAttributesManage : AuthenticatedPage
{
    private TransitPlace mPlace = null;
    public TransitPlace Place
    {
        get
        {
            if (mPlace == null)
            {
                mPlace = PlaceService.GetPlaceById(SessionManager.Ticket, RequestId);
            }

            return mPlace;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            linkNew.NavigateUrl = string.Format("PlaceAttributeEdit.aspx?aid={0}", RequestId);
            placeLink.HRef = string.Format("PlaceView.aspx?id={0}", RequestId);
            placeImage.Src = string.Format("PlacePictureThumbnail.aspx?id={0}", Place.PictureId);
            placeName.Text = Render(Place.Name);

            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                GetData(sender, e);
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
        gridManage.VirtualItemCount = PlaceService.GetPlaceAttributesCountById(RequestId);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private enum Cells
    {
        id = 0
    };

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageSize = gridManage.PageSize;
            options.PageNumber = gridManage.CurrentPageIndex;
            gridManage.DataSource = PlaceService.GetPlaceAttributesById(
                RequestId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    PlaceService.DeletePlaceAttribute(SessionManager.Ticket, id);
                    ReportInfo("Place attribute deleted.");
                    gridManage.CurrentPageIndex = 0;
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
