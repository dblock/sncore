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
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Specialized;

public partial class AccountFeedsView : Page
{
    private LocationSelectorCountryStateCity mLocationSelector = null;

    public LocationSelectorCountryStateCity LocationSelector
    {
        get
        {
            if (mLocationSelector == null)
            {
                mLocationSelector = new LocationSelectorCountryStateCity(
                    this, true, inputCountry, inputState, inputCity);
            }

            return mLocationSelector;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            LocationSelector.SelectLocation(sender, new LocationEventArgs(Request));
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("All Blogs", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    private SyndicationService.TransitAccountFeedQueryOptions GetQueryOptions()
    {
        SyndicationService.TransitAccountFeedQueryOptions options = new SyndicationService.TransitAccountFeedQueryOptions();
        options.PicturesOnly = false;
        options.PublishedOnly = true;
        options.WithFeedItemsOnly = true;
        options.City = inputCity.Text;
        options.Country = inputCountry.SelectedValue;
        options.State = inputState.SelectedValue;
        options.SortAscending = true;
        options.SortOrder = "Name";
        return options;
    }

    private void GetData(object sender, EventArgs e)
    {
        SyndicationService.TransitAccountFeedQueryOptions options = GetQueryOptions();

        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<SyndicationService.TransitAccountFeed, SyndicationService.ServiceQueryOptions, SyndicationService.TransitAccountFeedQueryOptions>(
            options, SessionManager.SyndicationService.GetAllAccountFeedsCount);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        SyndicationService.TransitAccountFeedQueryOptions options = GetQueryOptions();

        SyndicationService.ServiceQueryOptions serviceoptions = new SyndicationService.ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<SyndicationService.TransitAccountFeed, SyndicationService.ServiceQueryOptions, SyndicationService.TransitAccountFeedQueryOptions>(
            options, serviceoptions, SessionManager.SyndicationService.GetAllAccountFeeds);
    }

    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        Redirect(string.Format("AccountFeedsView.aspx?{0}", e.CommandArgument));
    }
}
