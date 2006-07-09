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

public partial class AccountsView : AccountPersonPage
{
    public class SelectLocationEventArgs : EventArgs
    {
        public string Country;
        public string State;
        public string City;

        public SelectLocationEventArgs(TransitAccount account)
            : this(account.Country, account.State, account.City)
        {

        }

        public SelectLocationEventArgs(
            string country,
            string state,
            string city)
        {
            Country = country;
            State = state;
            City = city;
        }
    }

    private AccountActivityQueryOptions mOptions = null;

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(search);

            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                ArrayList countries = new ArrayList();
                countries.Add(new TransitCountry());
                countries.AddRange(LocationService.GetCountries());

                ArrayList states = new ArrayList();
                states.Add(new TransitState());

                inputCountry.DataSource = countries;
                inputCountry.DataBind();

                inputState.DataSource = states;
                inputState.DataBind();

                linkLocal.Visible = SessionManager.IsLoggedIn && !string.IsNullOrEmpty(SessionManager.Account.City);

                if (SessionManager.IsLoggedIn)
                {
                    linkLocal.Text = string.Format("&#187; All {0} People", Renderer.Render(SessionManager.Account.City)); 
                    SelectLocation(sender, new SelectLocationEventArgs(
                        Request["country"],
                        Request["state"],
                        Request["city"]));
                }

                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void search_Click(object sender, EventArgs e)
    {
        try
        {
            GetData();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private void GetData()
    {
        gridManage.CurrentPage = 0;
        gridManage.VirtualItemCount = SocialService.GetAccountActivityCount(QueryOptions);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
        labelCount.Text = string.Format("{0} {1}",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount != 1 ? "people" : "person");
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ArrayList states = new ArrayList();
            states.Add(new TransitState());
            states.AddRange(LocationService.GetStatesByCountry(inputCountry.SelectedValue));
            inputState.DataSource = states;
            inputState.DataBind();
            panelCountryState.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private AccountActivityQueryOptions QueryOptions
    {
        get
        {
            if (mOptions == null)
            {
                mOptions = new AccountActivityQueryOptions();
                mOptions.SortAscending = bool.Parse(listboxSelectOrderBy.SelectedValue);
                mOptions.SortOrder = listboxSelectSortOrder.SelectedValue;
                mOptions.PicturesOnly = checkboxPicturesOnly.Checked;
                mOptions.City = inputCity.Text;
                mOptions.Country = inputCountry.SelectedValue;
                mOptions.State = inputState.SelectedValue;
                mOptions.Name = inputName.Text;
                mOptions.Email = inputEmailAddress.Text;
            }
            return mOptions;
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            AccountActivityQueryOptions options = QueryOptions;

            linkRss.NavigateUrl = linkRelRss.Attributes["href"] =
                string.Format("AccountsRss.aspx?order={0}&asc={1}&pictures={2}&city={3}&country={4}&state={5}&name={6}&email={7}",
                    QueryOptions.SortOrder,
                    QueryOptions.SortAscending,
                    QueryOptions.PicturesOnly,
                    QueryOptions.City,
                    QueryOptions.Country,
                    QueryOptions.State,
                    Renderer.UrlEncode(QueryOptions.Name),
                    Renderer.UrlEncode(QueryOptions.Email));

            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageSize = gridManage.PageSize;
            serviceoptions.PageNumber = gridManage.CurrentPage;
            gridManage.DataSource = SocialService.GetAccountActivity(options, serviceoptions);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void SelectLocation(object sender, SelectLocationEventArgs e)
    {
        try
        {
            inputCountry.ClearSelection();
            inputCountry.Items.FindByValue(e.Country).Selected = true;
            inputCountry_SelectedIndexChanged(sender, e);
            inputState.ClearSelection();
            inputState.Items.FindByValue(e.State).Selected = true;
            inputCity.Text = e.City;
        }
        catch
        {

        }
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SessionManager.IsLoggedIn)
                return;

            inputName.Text = string.Empty;
            inputCity.Text = string.Empty;
            SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
            GetData();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkAll_Click(object sender, EventArgs e)
    {
        try
        {
            inputCountry.ClearSelection();
            inputState.ClearSelection();
            inputCity.Text = string.Empty;
            inputName.Text = string.Empty;
            GetData();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        try
        {
            panelSearch.Visible = !panelSearch.Visible;
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
