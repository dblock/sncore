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
using Wilco.Web.UI;
using System.Drawing;
using System.Collections.Generic;
using SnCore.WebServices;
using LocationService;

public partial class AccountCitiesControl : Control
{
    public event CommandEventHandler SelectedChanged;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IList<TransitAccountCity> allcities = SessionManager.GetCollection<TransitAccountCity, LocationService.ServiceQueryOptions>(
                null, SessionManager.LocationService.GetAccountCities);

            List<TransitAccountCity> cities = new List<TransitAccountCity>(Max);

            TransitAccountCity t_all = new TransitAccountCity();
            t_all.Name = "All";
            cities.Add(t_all);

            if (SessionManager.IsLoggedIn && ! string.IsNullOrEmpty(SessionManager.Account.City))
            {
                TransitAccountCity t_city = new TransitAccountCity();
                t_city.Name = SessionManager.Account.City;
                t_city.Country = SessionManager.Account.Country;
                t_city.State = SessionManager.Account.State;
                cities.Add(t_city);
            }

            IEnumerator<TransitAccountCity> enumerator = allcities.GetEnumerator();
            while (enumerator.MoveNext() && cities.Count < Max)
            {
                if (SessionManager.IsLoggedIn && enumerator.Current.Name == SessionManager.Account.City)
                    continue;

                cities.Add(enumerator.Current);
            }

            listCities.DataSource = cities;
            listCities.DataBind();
        }
    }

    public string SelectedValue
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "City", string.Empty);
        }
        set
        {
            ViewState["City"] = value;
        }
    }

    public int Max
    {
        get
        {
            return listCities.RepeatColumns;
        }
        set
        {
            listCities.RepeatColumns = value;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    public void link_Command(object sender, CommandEventArgs e)
    {
        SelectedValue = e.CommandArgument.ToString();

        if (SelectedChanged != null)
        {
            SelectedChanged(sender, e);
        }
    }
}
