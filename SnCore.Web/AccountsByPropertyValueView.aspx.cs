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

public partial class AccountsByPropertyValueView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(GroupName, Request, string.Format("AccountsByPropertyValueView.aspx?GroupName={0}&PropertyName=&PropertyValue=", GroupName)));
            sitemapdata.Add(new SiteMapDataAttributeNode(PropertyName, Request, string.Format("AccountsByPropertyValueView.aspx?GroupName={0}&PropertyName={1}&PropertyValue=", GroupName, PropertyName)));
            sitemapdata.Add(new SiteMapDataAttributeNode(PropertyValue, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccount, string, string, string>(
            GroupName, PropertyName, PropertyValue, SessionManager.AccountService.GetAccountsByPropertyValueCount);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
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

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
        gridManage.DataSource = SessionManager.GetCollection<TransitAccount, string, string, string>(
            Request["GroupName"], Request["PropertyName"], Request["PropertyValue"], options,
            SessionManager.AccountService.GetAccountsByPropertyValue);
    }
}
