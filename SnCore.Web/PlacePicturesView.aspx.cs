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
using SnCore.Services;
using SnCore.WebServices;

public partial class PlacePicturesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitPlace a = PlaceService.GetPlaceById(RequestId);
                    linkPlace.Text = Renderer.Render(a.Name);
                    this.Title = string.Format("{0} Pictures", Renderer.Render(a.Name));
                    linkPlace.NavigateUrl = "PlaceView.aspx?id=" + a.Id;
                    listView.DataSource = PlaceService.GetPlacePicturesById(RequestId, null);
                    listView.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
