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
using SnCore.SiteMap;
using nStuff.UpdateControls;
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

        LocationSelector.CountryChanged += new EventHandler(LocationSelector_CountryChanged);
        LocationSelector.StateChanged += new EventHandler(LocationSelector_StateChanged);
        LocationSelector.CityChanged += new EventHandler(LocationSelector_CityChanged);

        ((SnCoreMasterPage)Master).History.Navigate += new HistoryEventHandler(History_Navigate);
        if (!IsPostBack)
        {
            linkLocal.Visible = SessionManager.IsLoggedIn && !string.IsNullOrEmpty(SessionManager.Account.City);

            if (SessionManager.IsLoggedIn)
            {
                linkLocal.Text = string.Format("&#187; All {0} Blogs", Renderer.Render(SessionManager.Account.City));
            }

            if (LocationSelector.SelectLocation(sender, new LocationEventArgs(Request)))
            {
                panelSearchInternal.Visible = true;
            }

            bool picturesOnly = false;
            if (bool.TryParse(Request["pictures"], out picturesOnly))
            {
                checkboxPicturesOnly.Checked = picturesOnly;
                panelSearchInternal.Visible = true;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            {
                inputName.Text = Request.QueryString["name"];
                panelSearchInternal.Visible = true;
            }

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("All Blogs", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(search);
    }

    void LocationSelector_StateChanged(object sender, EventArgs e)
    {
        panelCountryState.Update();
        panelCity.Update();
    }

    void LocationSelector_CountryChanged(object sender, EventArgs e)
    {
        panelCountryState.Update();
    }

    private TransitAccountFeedQueryOptions GetQueryOptions()
    {
        TransitAccountFeedQueryOptions options = new TransitAccountFeedQueryOptions();
        options.PicturesOnly = checkboxPicturesOnly.Checked;
        options.PublishedOnly = true;
        options.WithFeedItemsOnly = true;
        options.City = inputCity.Text;
        options.Country = inputCountry.SelectedValue;
        options.State = inputState.SelectedValue;
        options.Name = inputName.Text;
        return options;
    }

    void History_Navigate(object sender, HistoryEventArgs e)
    {
        string s = Encoding.Default.GetString(Convert.FromBase64String(e.EntryName));
        if (!string.IsNullOrEmpty(s))
        {
            NameValueCollection args = Renderer.ParseQueryString(s);
            gridManage.CurrentPageIndex = int.Parse(args["page"]);
        }
        else
        {
            gridManage.CurrentPageIndex = 0;
        }

        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    private void GetData(object sender, EventArgs e)
    {
        TransitAccountFeedQueryOptions options = GetQueryOptions();

        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountFeed, TransitAccountFeedQueryOptions>(
            options, SessionManager.SyndicationService.GetAllAccountFeedsCount);

        TransitAccountFeedItemQueryOptions feeditem_options = new TransitAccountFeedItemQueryOptions();
        feeditem_options.City = options.City;
        feeditem_options.Country = options.Country;
        feeditem_options.PublishedOnly = options.PublishedOnly;
        feeditem_options.State = options.State;
        feeditem_options.AccountFeedName = options.Name;
        feeditem_options.AccountFeedName = inputName.Text;

        int feedItemsCount = SessionManager.GetCount<TransitAccountFeedItem, TransitAccountFeedItemQueryOptions>(
            feeditem_options, SessionManager.SyndicationService.GetAllAccountFeedItemsCount);

        string feeds_queryargs = string.Format("city={0}&country={1}&state={2}",
            Renderer.UrlEncode(options.City),
            Renderer.UrlEncode(options.Country),
            Renderer.UrlEncode(options.State));

        labelCount.Text = string.Format("{0} blog{1} with <a href='AccountFeedItemsView.aspx?{4}'>{2} {3}</a>",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount == 1 ? string.Empty : "s",
            feedItemsCount, feedItemsCount == 1 ? "post" : "posts",
            feeds_queryargs);

        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        TransitAccountFeedQueryOptions options = GetQueryOptions();

        string queryargs = string.Format("order={0}&asc={1}&city={2}&country={3}&state={4}&name={5}&pictures={6}&page={7}",
                Renderer.UrlEncode(options.SortOrder),
                Renderer.UrlEncode(options.SortAscending),
                Renderer.UrlEncode(options.City),
                Renderer.UrlEncode(options.Country),
                Renderer.UrlEncode(options.State),
                Renderer.UrlEncode(options.Name),
                Renderer.UrlEncode(options.PicturesOnly),
                gridManage.CurrentPageIndex);

        Title = titleBlogs.Text = (string.IsNullOrEmpty(options.City)
            ? titleBlogs.DefaultText
            : string.Format("{0}: {1}", titleBlogs.DefaultText, options.City));

        if (IsPostBack)
        {
            Title = string.Format("{0} - {1}", SessionManager.GetCachedConfiguration(
                "SnCore.Title", "SnCore"), titleBlogs.Text);
        }

        linkRelRss.NavigateUrl = string.Format("AccountFeedsRss.aspx?{0}", queryargs);
        linkPermalink.NavigateUrl = string.Format("AccountFeedsView.aspx?{0}", queryargs);

        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountFeed, TransitAccountFeedQueryOptions>(
            options, serviceoptions, SessionManager.SyndicationService.GetAllAccountFeeds);

        if (!(e is HistoryEventArgs)) ((SnCoreMasterPage)Master).History.AddEntry(Convert.ToBase64String(Encoding.Default.GetBytes(queryargs)));

        panelLinks.Update();
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
        panelSearch.Update();
    }

    void LocationSelector_CityChanged(object sender, EventArgs e)
    {
        panelCountryState.Update();
        panelCity.Update();
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsLoggedIn)
            return;

        inputName.Text = string.Empty;
        inputCity.Text = string.Empty;
        LocationSelector.SelectLocation(sender, new LocationEventArgs(SessionManager.Account));
        GetData(sender, e);
        panelSearch.Update();
    }

    public void linkAll_Click(object sender, EventArgs e)
    {
        inputCountry.ClearSelection();
        inputState.ClearSelection();
        inputCity.Text = string.Empty;
        inputName.Text = string.Empty;
        GetData(sender, e);
        panelSearch.Update();
    }

    public void atoz_SelectedChanged(object sender, CommandEventArgs e)
    {
        panelSearch.Update();
        inputName.Text = e.CommandArgument.ToString();
        GetData(sender, e);
    }

    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        panelSearch.Update();
        NameValueCollection args = Renderer.ParseQueryString(e.CommandArgument.ToString());
        LocationSelector.SelectLocation(sender, new LocationEventArgs(args));
        GetData(sender, e);
    }
}
