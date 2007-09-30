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
using SnCore.Tools.Web;
using SnCore.WebServices;
using SnCore.SiteMap;
using PlaceService;

public partial class PlacePicturesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (RequestId == 0)
        {
            throw new Exception("Missing place.");
        }

        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            TransitPlace p = SessionManager.GetInstance<TransitPlace, PlaceService.ServiceQueryOptions, int>(
                RequestId, SessionManager.PlaceService.GetPlaceById);

            placeName.Text = this.Title = string.Format("{0}'s Pictures", Renderer.Render(p.Name));

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("PlaceView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitPlacePicture, PlaceService.ServiceQueryOptions, int>(
            RequestId, SessionManager.PlaceService.GetPlacePicturesCount);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        PlaceService.ServiceQueryOptions options = new PlaceService.ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitPlacePicture, PlaceService.ServiceQueryOptions, int>(
            RequestId, options, SessionManager.PlaceService.GetPlacePictures);
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
