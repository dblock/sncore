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
using SnCore.SiteMap;

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

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(GroupName, Request, string.Format("PlacesByPropertyValueView.aspx?GroupName={0}", Renderer.UrlEncode(GroupName))));
                sitemapdata.Add(new SiteMapDataAttributeNode(PropertyName, Request, string.Format("PlacesByPropertyValueView.aspx?GroupName={0}&PropertyName={1}", Renderer.UrlEncode(GroupName), Renderer.UrlEncode(PropertyName))));
                sitemapdata.Add(new SiteMapDataAttributeNode(PropertyValue, Request, string.Format("PlacesByPropertyValueView.aspx?GroupName={0}&PropertyName={1}&PropertyValue={2}", Renderer.UrlEncode(GroupName), Renderer.UrlEncode(PropertyName), Renderer.UrlEncode(PropertyValue))));
                StackSiteMap(sitemapdata);
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
            string result = Request["GroupName"];
            if (string.IsNullOrEmpty(result)) return string.Empty;
            return result;
        }
    }

    public string PropertyName
    {
        get
        {
            string result = Request["PropertyName"];
            if (string.IsNullOrEmpty(result)) return string.Empty;
            return result;
        }
    }

    public string PropertyValue
    {
        get
        {
            string result = Request["PropertyValue"];
            if (string.IsNullOrEmpty(result)) return string.Empty;
            return result;
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        titlePlacesByProperty.Text = string.Format("{0}: {1}", Renderer.Render(PropertyName), Renderer.Render(PropertyValue));
        gridManage.CurrentPageIndex = 0;
        object[] args = { GroupName, PropertyName, PropertyValue };
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            SessionManager.PlaceService, "GetPlacesByPropertyValueCount", args);
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
            gridManage.DataSource = SessionManager.GetCachedCollection<TransitPlace>(
                SessionManager.PlaceService, "GetPlacesByPropertyValue", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
