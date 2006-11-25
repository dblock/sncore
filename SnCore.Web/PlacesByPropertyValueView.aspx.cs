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
using System.Collections.Generic;

public partial class PlacesByPropertyValueView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
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

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    public string GroupName
    {
        get
        {
            return Request["GroupName"];
        }
    }

    public string PropertyName
    {
        get
        {
            return Request["PropertyName"];
        }
    }

    public string PropertyValue
    {
        get
        {
            return Request["PropertyValue"];
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        title.Text = string.Format("{0}: {1}", Renderer.Render(PropertyName), Renderer.Render(PropertyValue));
        gridManage.CurrentPageIndex = 0;
        object[] args = { GroupName, PropertyName, PropertyValue };
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(PlaceService, "GetPlacesByPropertyValueCount", args);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    protected override void OnInit(EventArgs e)
    {
        ppvs.PropertyName = PropertyName;
        ppvs.GroupName = GroupName;
        base.OnInit(e);
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
            object[] args = { GroupName, PropertyName, PropertyValue, serviceoptions };
            gridManage.DataSource = SessionManager.GetCachedCollection<TransitPlace>(PlaceService, "GetPlacesByPropertyValue", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
