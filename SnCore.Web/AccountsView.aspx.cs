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
using SnCore.SiteMap;
using System.Collections.Generic;
using nStuff.UpdateControls;
using System.Text;
using System.Collections.Specialized;

[SiteMapDataAttribute("People")]
public partial class AccountsView : AccountPersonPage
{
    public AccountsView()
    {
        mIsMobileEnabled = true;
    }

    public class LocationWithOptionsEventArgs : LocationEventArgs
    {
        public string Name;
        public string Email;
        public string BloggersOnly;
        public string PicturesOnly;

        public LocationWithOptionsEventArgs(TransitAccount account)
            : base(account)
        {

        }

        public LocationWithOptionsEventArgs(HttpRequest request)
            : base(request)
        {
            Name = request["name"];
            Email = request["email"];
            PicturesOnly = request["pictures"];
            BloggersOnly = request["bloggers"];
        }

        public LocationWithOptionsEventArgs(NameValueCollection coll)
            : base(coll)
        {
            Name = coll["name"];
            Email = coll["email"];
            PicturesOnly = coll["pictures"];
            BloggersOnly = coll["bloggers"];
        }
    }

    public class LocationSelectorWithOptions : LocationSelectorCountryStateCity
    {
        private CheckBox mBloggersOnly;
        private CheckBox mPicturesOnly;

        public LocationSelectorWithOptions(
            Page page,
            bool empty,
            DropDownList country,
            DropDownList state,
            DropDownList city,
            CheckBox bloggersonly,
            CheckBox picturesonly)
            :
            base(page, empty, country, state, city)
        {
            mBloggersOnly = bloggersonly;
            mPicturesOnly = picturesonly;
        }

        public bool SelectLocation(object sender, LocationWithOptionsEventArgs e)
        {
            bool result = base.SelectLocation(sender, e);

            if (mPicturesOnly != null && !string.IsNullOrEmpty(e.PicturesOnly))
            {
                bool picturesonly = false;
                if (bool.TryParse(e.PicturesOnly, out picturesonly))
                {
                    mPicturesOnly.Checked = picturesonly;
                    result = true;
                }
            }

            if (mBloggersOnly != null && !string.IsNullOrEmpty(e.BloggersOnly))
            {
                bool bloggersonly = false;
                if (bool.TryParse(e.BloggersOnly, out bloggersonly))
                {
                    mBloggersOnly.Checked = bloggersonly;
                    result = true;
                }
            }

            return result;
        }
    }

    private LocationSelectorWithOptions mLocationSelector = null;

    public LocationSelectorWithOptions LocationSelector
    {
        get
        {
            if (mLocationSelector == null)
            {
                mLocationSelector = new LocationSelectorWithOptions(
                    this, true, inputCountry, inputState, inputCity, checkboxBloggersOnly, checkboxPicturesOnly);
            }

            return mLocationSelector;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(search);

        LocationSelector.CountryChanged += new EventHandler(LocationSelector_CountryChanged);
        LocationSelector.StateChanged += new EventHandler(LocationSelector_StateChanged);
        LocationSelector.CityChanged += new EventHandler(LocationSelector_CityChanged);

        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        ((SnCoreMasterPage)Master).History.Navigate += new HistoryEventHandler(History_Navigate);

        if (!IsPostBack)
        {
            if (LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(Request)))
            {
                panelSearchInternal.Visible = true;
            }

            linkLocal.Visible = SessionManager.IsLoggedIn && !string.IsNullOrEmpty(SessionManager.Account.City);

            if (SessionManager.IsLoggedIn)
            {
                linkLocal.Text = string.Format("&#187; All {0} People", Renderer.Render(SessionManager.Account.City));
            }

            GetData(sender, e);
        }
    }

    void History_Navigate(object sender, HistoryEventArgs e)
    {
        string s = Encoding.Default.GetString(Convert.FromBase64String(e.EntryName));
        if (!string.IsNullOrEmpty(s))
        {
            NameValueCollection args = Renderer.ParseQueryString(s);
            LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(args));
            gridManage.CurrentPageIndex = int.Parse(args["page"]);
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
        }
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
        panelGrid.Update();
    }

    public int AccountsCount
    {
        get
        {
            return SessionManager.GetCount<TransitAccount>(
                SessionManager.AccountService.GetAccountsCount);
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountActivity, AccountActivityQueryOptions>(
            GetQueryOptions(), SessionManager.SocialService.GetAccountActivityCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
        labelCount.Text = string.Format("{0}/{1} people",
            gridManage.VirtualItemCount,
            AccountsCount);
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<TransitState> states = new List<TransitState>();
        states.Add(new TransitState());
        states.AddRange(SessionManager.GetCollection<TransitState, string>(
            inputCountry.SelectedValue, (ServiceQueryOptions)null, SessionManager.LocationService.GetStatesByCountryName));
        inputState.DataSource = states;
        inputState.DataBind();
        panelCountryState.Update();
    }

    private AccountActivityQueryOptions GetQueryOptions()
    {
        AccountActivityQueryOptions options = new AccountActivityQueryOptions();
        options.SortAscending = bool.Parse(listboxSelectOrderBy.SelectedValue);
        options.SortOrder = listboxSelectSortOrder.SelectedValue;
        options.PicturesOnly = checkboxPicturesOnly.Checked;
        options.BloggersOnly = checkboxBloggersOnly.Checked;
        options.City = inputCity.Text;
        options.Country = inputCountry.SelectedValue;
        options.State = inputState.SelectedValue;
        options.Name = inputName.Text;
        options.Email = inputEmailAddress.Text;
        return options;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        AccountActivityQueryOptions options = GetQueryOptions();

        string args = string.Format("order={0}&asc={1}&pictures={2}&city={3}&country={4}&state={5}&name={6}&email={7}&bloggers={8}&page={9}",
            options.SortOrder,
            options.SortAscending,
            options.PicturesOnly,
            options.City,
            options.Country,
            options.State,
            Renderer.UrlEncode(options.Name),
            Renderer.UrlEncode(options.Email),
            options.BloggersOnly,
            gridManage.CurrentPageIndex);

        Title = titlePeople.Text = (string.IsNullOrEmpty(options.City)
            ? titlePeople.DefaultText
            : string.Format("{0}: {1}", titlePeople.DefaultText, options.City));

        if (IsPostBack)
        {
            Title = string.Format("{0} - {1}", SessionManager.GetCachedConfiguration(
                "SnCore.Title", "SnCore"), titlePeople.Text);
        }

        linkRelRss.NavigateUrl = string.Format("AccountsRss.aspx?{0}", args);
        linkPermalink.NavigateUrl = string.Format("AccountsView.aspx?{0}", args);

        if (!(e is HistoryEventArgs)) ((SnCoreMasterPage)Master).History.AddEntry(Convert.ToBase64String(Encoding.Default.GetBytes(args)));

        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountActivity, AccountActivityQueryOptions>(
            options, serviceoptions, SessionManager.SocialService.GetAccountActivity);

        panelLinks.Update();
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsLoggedIn)
            return;

        checkboxBloggersOnly.Checked = false;
        checkboxPicturesOnly.Checked = false;
        inputName.Text = string.Empty;
        inputCity.Text = string.Empty;
        inputEmailAddress.Text = string.Empty;
        LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(SessionManager.Account));
        GetData(sender, e);
        panelSearch.Update();
    }

    public void linkAll_Click(object sender, EventArgs e)
    {
        checkboxBloggersOnly.Checked = false;
        checkboxPicturesOnly.Checked = false;
        inputCountry.ClearSelection();
        inputState.ClearSelection();
        inputCity.Text = string.Empty;
        inputName.Text = string.Empty;
        inputEmailAddress.Text = string.Empty;
        GetData(sender, e);
        panelSearch.Update();
    }

    public void linkBloggers_Click(object sender, EventArgs e)
    {
        checkboxPicturesOnly.Checked = false;
        checkboxBloggersOnly.Checked = true;
        inputCountry.ClearSelection();
        inputState.ClearSelection();
        inputCity.Text = string.Empty;
        inputName.Text = string.Empty;
        inputEmailAddress.Text = string.Empty;
        GetData(sender, e);
        panelSearch.Update();
    }

    void LocationSelector_CityChanged(object sender, EventArgs e)
    {
        panelCountryState.Update();
        panelCity.Update();
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

    public void linkSearch_Click(object sender, EventArgs e)
    {
        panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
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
        LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(args));
        GetData(sender, e);
    }
}
