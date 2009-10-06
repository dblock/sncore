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
using SnCore.Services;
using SnCore.SiteMap;
using SnCore.Tools.Web.Html;
using System.Collections.Specialized;
using SnCore.WebControls;

[SiteMapDataAttribute("Blogs")]
public partial class AccountFeedItemsView : Page
{
    public AccountFeedItemsView()
    {
        mIsMobileEnabled = true;
    }

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

        ((SnCoreMasterPage)Master).ScriptManager.Navigate += new EventHandler<HistoryEventArgs>(History_Navigate);

        SetDefaultButton(search);
        if (!IsPostBack)
        {
            linkLocal.Visible = SessionManager.IsLoggedIn && !string.IsNullOrEmpty(SessionManager.Account.City);

            if (SessionManager.IsLoggedIn)
            {
                linkLocal.Text = string.Format("&#187; All {0} Blog Posts", Renderer.Render(SessionManager.Account.City));
            }

            if (LocationSelector.SelectLocation(sender, new LocationEventArgs(Request)))
            {
                panelSearchInternal.Visible = true;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["search"]))
            {
                inputSearch.Text = Request.QueryString["search"];
                panelSearchInternal.Visible = true;
            }

            GetData(sender, e);
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

    private TransitAccountFeedItemQueryOptions GetQueryOptions()
    {
        TransitAccountFeedItemQueryOptions options = new TransitAccountFeedItemQueryOptions();
        options.PublishedOnly = true;
        options.City = inputCity.Text;
        options.Country = inputCountry.SelectedValue;
        options.State = inputState.SelectedValue;
        options.Search = inputSearch.Text;
        return options;
    }

    void History_Navigate(object sender, HistoryEventArgs e)
    {
        if (e.State.HasKeys())
        {
            gridManage.CurrentPageIndex = int.Parse(e.State["page"]);
        }
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    private void GetData(object sender, EventArgs e)
    {
        TransitAccountFeedItemQueryOptions options = GetQueryOptions();

        gridManage.CurrentPageIndex = 0;

        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountFeedItem, TransitAccountFeedItemQueryOptions>(
                options, SessionManager.SyndicationService.GetAllAccountFeedItemsCount);

        Title = titleBlogPosts.Text = (string.IsNullOrEmpty(options.City)
            ? titleBlogPosts.DefaultText
            : string.Format("{0}: {1}", titleBlogPosts.DefaultText, options.City));

        if (IsPostBack)
        {
            Title = string.Format("{0} - {1}", SessionManager.GetCachedConfiguration(
                "SnCore.Title", "SnCore"), titleBlogPosts.Text);
        }

        TransitAccountFeedQueryOptions feed_options = new TransitAccountFeedQueryOptions();
        feed_options.PublishedOnly = true;
        feed_options.PicturesOnly = false;
        feed_options.City = options.City;
        feed_options.State = options.State;
        feed_options.Country = options.Country;
        feed_options.WithFeedItemsOnly = true;
        int feedsCount = SessionManager.GetCount<TransitAccountFeed, TransitAccountFeedQueryOptions>(
            feed_options, SessionManager.SyndicationService.GetAccountFeedsCount);

        string feeds_queryargs = string.Format("city={0}&country={1}&state={2}",
            Renderer.UrlEncode(options.City),
            Renderer.UrlEncode(options.Country),
            Renderer.UrlEncode(options.State));

        labelCount.Text = string.Format("{0} post{1} from <a href='AccountFeedsView.aspx?{4}'>{2} blog{3}</a>",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount == 1 ? string.Empty : "s",
            feedsCount, feedsCount == 1 ? string.Empty : "s",
            feeds_queryargs);

        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        TransitAccountFeedItemQueryOptions options = GetQueryOptions();

        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountFeedItem, TransitAccountFeedItemQueryOptions>(
            options, serviceoptions, SessionManager.SyndicationService.GetAllAccountFeedItems);

        string queryargs = string.Format("city={0}&country={1}&state={2}&search={3}&page={4}",
                Renderer.UrlEncode(options.City),
                Renderer.UrlEncode(options.Country),
                Renderer.UrlEncode(options.State),
                Renderer.UrlEncode(options.Search),
                gridManage.CurrentPageIndex);

        if (((SnCoreMasterPage)Master).ScriptManager.IsInAsyncPostBack &&
            !((SnCoreMasterPage)Master).ScriptManager.IsNavigating)
        {
            NameValueCollection history = new NameValueCollection();
            history.Add("city", options.City);
            history.Add("country", options.Country);
            history.Add("state", options.State);
            history.Add("search", options.Search);
            history.Add("page", gridManage.CurrentPageIndex.ToString());
            ((SnCoreMasterPage)Master).ScriptManager.AddHistoryPoint(history, Page.Title);
        }

        linkRelRss.NavigateUrl = string.Format("AccountFeedItemsRss.aspx?{0}", queryargs);
        linkPermalink.NavigateUrl = string.Format("AccountFeedItemsView.aspx?{0}", queryargs);

        panelLinks.Update();
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

    public string GetComments(int count)
    {
        if (count == 0) return "read and comment";
        else if (count == 1) return "read 1 comment";
        else return string.Format("read {0} comments", count);
    }

    public string GetSummary(string summary, string link)
    {
        Uri uri = null;
        Uri.TryCreate(link, UriKind.Absolute, out uri);
        Uri imgrewriteuri = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
        HtmlWriterOptions options = new HtmlWriterOptions();
        options.BaseHref = uri;
        options.RewriteImgSrc = imgrewriteuri;
        return Renderer.CleanHtml(summary, options);
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
        panelSearch.Update();
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsLoggedIn)
            return;

        inputSearch.Text = string.Empty;
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
        inputSearch.Text = string.Empty;
        GetData(sender, e);
        panelSearch.Update();
    }

    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        panelSearch.Update();
        NameValueCollection args = Renderer.ParseQueryString(e.CommandArgument.ToString());
        LocationSelector.SelectLocation(sender, new LocationEventArgs(args));
        GetData(sender, e);
    }
}
