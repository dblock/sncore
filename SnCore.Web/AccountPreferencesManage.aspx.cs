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
using System.Collections.Generic;
using System.Web.Caching;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class AccountPreferencesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Preferences", Request.Url));
            StackSiteMap(sitemapdata);

            DomainClass cs = SessionManager.GetDomainClass("Account");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputCity.MaxLength = cs["City"].MaxLengthInChars;
            inputSignature.MaxLength = cs["Signature"].MaxLengthInChars;

            inputName.Text = SessionManager.Account.Name;

            inputBirthday.SelectedDate = SessionManager.Account.Birthday;
            inputBirthday.DataBind();
            inputCity.Text = SessionManager.Account.City;
            inputTimeZone.SelectedTzIndex = SessionManager.Account.TimeZone;

            List<TransitCountry> countries = new List<TransitCountry>();
            if (SessionManager.Account.Country.Length == 0) countries.Add(new TransitCountry());
            string defaultcountry = SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            countries.AddRange(SessionManager.GetCollection<TransitCountry, string>(
                defaultcountry, (ServiceQueryOptions)null, SessionManager.LocationService.GetCountriesWithDefault));

            List<TransitState> states = new List<TransitState>();
            if (SessionManager.Account.State.Length == 0) states.Add(new TransitState());
            states.AddRange(SessionManager.GetCollection<TransitState, string>(
                SessionManager.Account.Country, (ServiceQueryOptions)null, SessionManager.LocationService.GetStatesByCountryName));

            inputCountry.DataSource = countries;
            inputCountry.DataBind();

            inputState.DataSource = states;
            inputState.DataBind();

            inputCountry.ClearSelection();
            inputCountry.Items.FindByValue(SessionManager.Account.Country).Selected = true;

            inputState.ClearSelection();
            inputState.Items.FindByValue(SessionManager.Account.State).Selected = true;

            inputSignature.Text = SessionManager.Account.Signature;

            groups.AccountId = SessionManager.Account.Id;

            accountredirect.TargetUri = string.Format("AccountView.aspx?id={0}", SessionManager.Account.Id);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccount ta = SessionManager.Account;
        ta.Birthday = inputBirthday.SelectedDate;
        ta.Name = inputName.Text;
        ta.City = inputCity.Text;
        ta.Country = inputCountry.SelectedValue;
        ta.State = inputState.SelectedValue;
        ta.TimeZone = inputTimeZone.SelectedTzIndex;
        ta.Signature = inputSignature.Text;

        if (ta.Signature.Length > inputSignature.MaxLength)
            throw new Exception(string.Format("Signature may not exceed {0} characters.", inputSignature.MaxLength));

        SessionManager.CreateOrUpdate<TransitAccount>(
            ta, SessionManager.AccountService.CreateOrUpdateAccount);
        Cache.Remove(string.Format("account:{0}", SessionManager.Ticket));
        ReportInfo("Profile saved.");
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<TransitState> states = new List<TransitState>();
        states.Add(new TransitState());
        states.AddRange(SessionManager.GetCollection<TransitState, string>(
            inputCountry.SelectedValue, (ServiceQueryOptions) null, 
            SessionManager.LocationService.GetStatesByCountryName));

        inputState.DataSource = states;
        inputState.DataBind();

        panelCountryState.Update();
    }
}
