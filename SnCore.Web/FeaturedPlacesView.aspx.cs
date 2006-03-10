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
using System.Text;
using SnCore.Services;
using SnCore.WebServices;
using System.Reflection;
using System.Collections.Generic;

public partial class FeaturedPlacesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SystemService.GetFeaturesCount("Place");
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();

        if (gridManage.VirtualItemCount == 0)
        {
            labelCount.Text = "No Featured Places";
        }
        else if (gridManage.VirtualItemCount == 1)
        {
            labelCount.Text = "1 Featured Place";
        }
        else
        {
            labelCount.Text = string.Format("{0} Featured Places!", gridManage.VirtualItemCount);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageSize = gridManage.PageSize;
            serviceoptions.PageNumber = gridManage.CurrentPageIndex;
            gridManage.DataSource = SystemService.GetFeatures("Place", serviceoptions);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetSummary(string summary)
    {
        string result = Renderer.RemoveHtml(summary);
        if (result.Length > 256) result = result.Substring(0, 256) + " ...";
        return result;
    }

    public TransitPlace GetPlace(int id)
    {
        TransitPlace a = (TransitPlace)Cache[string.Format("place:{0}", id)];
        if (a == null)
        {
            a = PlaceService.GetPlaceById(id);
            Cache.Insert(string.Format("place:{0}", id),
                a, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
        }

        return a;
    }
}
