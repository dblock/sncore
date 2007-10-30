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

[SiteMapDataAttribute("Blogs")]
public partial class AccountFeedItemsView : Page
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
        }
    }

    private SyndicationService.TransitAccountFeedItemQueryOptions GetQueryOptions()
    {
        SyndicationService.TransitAccountFeedItemQueryOptions options = new SyndicationService.TransitAccountFeedItemQueryOptions();
        options.PublishedOnly = true;
        options.City = inputCity.Text;
        options.Country = inputCountry.Text;
        options.State = inputState.Text;
        return options;
    }

    private void GetData(object sender, EventArgs e)
    {
        SyndicationService.TransitAccountFeedItemQueryOptions options = GetQueryOptions();

        gridManage.CurrentPageIndex = 0;

        gridManage.VirtualItemCount = SessionManager.GetCount<
            SyndicationService.TransitAccountFeedItem, SyndicationService.ServiceQueryOptions, SyndicationService.TransitAccountFeedItemQueryOptions>(
                options, SessionManager.SyndicationService.GetAllAccountFeedItemsCount);

        SyndicationService.TransitAccountFeedQueryOptions feed_options = new SyndicationService.TransitAccountFeedQueryOptions();
        feed_options.PublishedOnly = true;
        feed_options.PicturesOnly = false;
        feed_options.City = options.City;
        feed_options.State = options.State;
        feed_options.Country = options.Country;
        feed_options.WithFeedItemsOnly = true;

        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        SyndicationService.TransitAccountFeedItemQueryOptions options = GetQueryOptions();
        SyndicationService.ServiceQueryOptions serviceoptions = new SyndicationService.ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<SyndicationService.TransitAccountFeedItem, SyndicationService.ServiceQueryOptions, SyndicationService.TransitAccountFeedItemQueryOptions>(
            options, serviceoptions, SessionManager.SyndicationService.GetAllAccountFeedItems);
    }

    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        Redirect(string.Format("AccountFeedItemsView.aspx?{0}", e.CommandArgument));
    }

    public string GetValue(string s, string defaultvalue)
    {
        return string.IsNullOrEmpty(s) ? defaultvalue : s;
    }
}
