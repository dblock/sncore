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
using System.Collections.Generic;
using SnCore.Tools.Web;
using Wilco.Web.UI;
using SnCore.WebServices;

public partial class PlacePicturesViewControl : Control
{
    public int PlaceId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "PlaceId", 0);
        }
        set
        {
            ViewState["PlaceId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);

        if (!IsPostBack)
        {
            GetPicturesData(sender, e);
            linkUpload.NavigateUrl = string.Format("PlacePicturesManage.aspx?id={0}", PlaceId);
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCount<TransitPlacePicture, int>(
            PlaceId, SessionManager.PlaceService.GetPlacePicturesCount);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
        noPictures.Visible = (picturesView.Items.Count == 0);
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
        picturesView.DataSource = SessionManager.GetCollection<TransitPlacePicture, int>(
            PlaceId, options, SessionManager.PlaceService.GetPlacePictures);
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
