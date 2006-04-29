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

public partial class AccountEventsToday : Page
{
    public class SelectLocationEventArgs : EventArgs
    {
        public string Country;
        public string State;
        public string City;
        public string Type;

        public SelectLocationEventArgs(TransitAccount account)
            : this(account.Country, account.State, account.City, string.Empty)
        {

        }

        public SelectLocationEventArgs(HttpRequest request)
            : this(request["country"], request["state"], request["city"], request["type"])
        {

        }

        public SelectLocationEventArgs(
            string country,
            string state,
            string city,
            string type)
        {
            Country = country;
            State = state;
            City = city;
            Type = type;
        }
    }

    private TransitAccountEventInstanceQueryOptions mOptions = null;

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(search);
            if (!IsPostBack)
            {
                ArrayList types = new ArrayList();
                types.Add(new TransitAccountEventType());
                types.AddRange(EventService.GetAccountEventTypes());
                inputType.DataSource = types;
                inputType.DataBind();

                ArrayList countries = new ArrayList();
                countries.Add(new TransitCountry());
                countries.AddRange(LocationService.GetCountries());
                inputCountry.DataSource = countries;
                inputCountry.DataBind();

                if (SessionManager.IsLoggedIn && (Request.QueryString.Count == 0))
                {
                    SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
                }
                else
                {
                    SelectLocation(sender, new SelectLocationEventArgs(Request));
                }

                SelectWeek();
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private void SelectWeek()
    {
        DateTime adjustedNow = base.Adjust(DateTime.UtcNow);

        calendarEvents.VisibleDate = adjustedNow;

        do
        {
            calendarEvents.SelectedDates.Add(adjustedNow.Date);
            adjustedNow = adjustedNow.AddDays(1);
        } while (adjustedNow.DayOfWeek != DayOfWeek.Monday);
    }

    private void GetData()
    {
        mOptions = null;

        gridManage.AllowPaging = false;
        gridManage.CurrentPageIndex = 0;
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            TransitAccountEventInstanceQueryOptions options = QueryOptions;
            gridManage.DataSource = EventService.GetAccountEventInstances(SessionManager.Ticket, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
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
            inputState_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }


    public void inputState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ArrayList cities = new ArrayList();
            cities.Add(new TransitCity());
            cities.AddRange(LocationService.GetCitiesByLocation(inputCountry.SelectedValue, inputState.SelectedValue));
            inputCity.DataSource = cities;
            inputCity.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private TransitAccountEventInstanceQueryOptions QueryOptions
    {
        get
        {
            if (mOptions == null)
            {
                mOptions = new TransitAccountEventInstanceQueryOptions();
                mOptions.City = inputCity.Text;
                mOptions.Country = inputCountry.SelectedValue;
                mOptions.State = inputState.SelectedValue;
                mOptions.Type = inputType.SelectedValue;
                mOptions.StartDateTime = DateTime.MinValue;
                mOptions.EndDateTime = DateTime.MaxValue;
                if (calendarEvents.SelectedDates.Count > 0)
                {
                    foreach (DateTime dt in calendarEvents.SelectedDates)
                    {
                        DateTime d = base.ToUTC(dt);

                        if (mOptions.StartDateTime == DateTime.MinValue || d < mOptions.StartDateTime)
                            mOptions.StartDateTime = d;

                        // include the entire day (adddays(1))
                        if (mOptions.EndDateTime == DateTime.MaxValue || d.AddDays(1) > mOptions.EndDateTime)
                            mOptions.EndDateTime = d.AddDays(1);
                    }
                }
                else
                {
                    mOptions.StartDateTime = mOptions.EndDateTime = calendarEvents.SelectedDate;
                }

                if (mOptions.StartDateTime == mOptions.EndDateTime)
                {
                    mOptions.EndDateTime = mOptions.EndDateTime.AddDays(1);
                }
            }
            return mOptions;
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
            inputState_SelectedIndexChanged(sender, e);
            inputCity.ClearSelection();
            inputCity.Items.FindByValue(e.City).Selected = true;
            inputType.ClearSelection();
            inputType.Items.FindByValue(e.Type).Selected = true;
        }
        catch
        {

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

    protected void calendarEvents_SelectionChanged(object sender, EventArgs e)
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

    public void linkShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            inputCountry.ClearSelection();
            inputState.ClearSelection();
            inputCity.ClearSelection();
            inputType.ClearSelection();
            SelectWeek();
            GetData();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
