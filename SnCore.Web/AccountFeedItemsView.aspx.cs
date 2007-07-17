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
using nStuff.UpdateControls;
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

        LocationSelector.CountryChanged += new EventHandler(LocationSelector_CountryChanged);
        LocationSelector.StateChanged += new EventHandler(LocationSelector_StateChanged);
        LocationSelector.CityChanged += new EventHandler(LocationSelector_CityChanged);

        ((SnCoreMasterPage)Master).History.Navigate += new HistoryEventHandler(History_Navigate);
        SetDefaultButton(search);
        if (!IsPostBack)
        {
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
        string s = Encoding.Default.GetString(Convert.FromBase64String(e.EntryName));
        if (!string.IsNullOrEmpty(s))
        {
            NameValueCollection args = Renderer.ParseQueryString(s);
            inputSearch.Text = args["q"];
            gridManage.CurrentPageIndex = int.Parse(args["page"]);
        }
        else
        {
            inputSearch.Text = Request["q"];
            gridManage.CurrentPageIndex = 0;
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

        TransitAccountFeedQueryOptions feed_options = new TransitAccountFeedQueryOptions();
        feed_options.PublishedOnly = false;
        feed_options.PicturesOnly = false;
        feed_options.City = options.City;
        feed_options.State = options.State;
        feed_options.Country = options.Country;
        feed_options.WithFeedItemsOnly = true;
        int feedsCount = SessionManager.GetCount<TransitAccountFeed, TransitAccountFeedQueryOptions>(
            feed_options, SessionManager.SyndicationService.GetAllAccountFeedsCount);

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

        if (!(e is HistoryEventArgs)) ((SnCoreMasterPage)Master).History.AddEntry(Convert.ToBase64String(Encoding.Default.GetBytes(queryargs)));

        linkRelRss.NavigateUrl = string.Format("AccountFeedItemsRss.aspx?{0}", queryargs);
        linkPermalink.NavigateUrl = string.Format("AccountFeedItemsView.aspx?{0}", queryargs);
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
        return Renderer.CleanHtml(summary, uri, imgrewriteuri);
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
}
