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

[SiteMapDataAttribute("Me Me")]
public partial class AccountPreferencesManage : AuthenticatedPage
{
    public class AccountNumbers
    {
        public int FirstDegreeCount;
        public int SecondDegreeCount;
        public int AllCount;

        public int NewCount
        {
            get
            {
                return AllCount - FirstDegreeCount - 1; // all minus first degree minus self
            }
        }

        public int PostsCount;
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            inputName.Text = SessionManager.Account.Name;

            accountName.Text = string.Format("Hello, {0}!", Renderer.Render(SessionManager.Account.Name));
            accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", SessionManager.Account.PictureId);

            AccountNumbers numbers = (AccountNumbers)Cache[string.Format("accountnumbers:{0}", SessionManager.Ticket)];
            if (numbers == null)
            {
                numbers = new AccountNumbers();
                numbers.FirstDegreeCount = SessionManager.SocialService.GetFirstDegreeCountById(SessionManager.Ticket, SessionManager.AccountId);
                numbers.SecondDegreeCount = SessionManager.SocialService.GetNDegreeCountById(SessionManager.Ticket, SessionManager.AccountId, 2);
                numbers.AllCount = SessionManager.AccountService.GetAccountsCount(SessionManager.Ticket);

                DiscussionQueryOptions options = new DiscussionQueryOptions();
                options.AccountId = SessionManager.Account.Id;
                numbers.PostsCount = SessionManager.DiscussionService.GetUserDiscussionThreadsCount(
                    SessionManager.Ticket, options);

                Cache.Insert(string.Format("accountnumbers:{0}", SessionManager.Ticket),
                    numbers, null, Cache.NoAbsoluteExpiration, SessionManager.DefaultCacheTimeSpan);
            }

            accountFirstDegree.Text = string.Format("{0} friend{1} in your personal network",
                numbers.FirstDegreeCount,
                numbers.FirstDegreeCount != 1 ? "s" : string.Empty);

            accountSecondDegree.Text = string.Format("{0} friend{1} in your extended network",
                numbers.SecondDegreeCount,
                numbers.SecondDegreeCount != 1 ? "s" : string.Empty);

            accountAllDegrees.Text = string.Format("{0} {1} to make new friends with",
                numbers.NewCount > 0 ? numbers.NewCount.ToString() : "no",
                numbers.NewCount != 1 ? "people" : "person");

            accountDiscussionThreads.Text = string.Format("{0} discussion post{1}",
                numbers.PostsCount, numbers.PostsCount != 1 ? "s" : string.Empty);

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

            inputCountry.Items.FindByValue(SessionManager.Account.Country).Selected = true;
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

        SessionManager.AccountService.CreateOrUpdateAccount(SessionManager.Ticket, ta);
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
