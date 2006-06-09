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
        try
        {
            if (!IsPostBack)
            {
                inputName.Text = SessionManager.Account.Name;

                List<TransitAccountProfile> profiles = AccountService.GetAccountProfilesById(SessionManager.Account.Id);
                foreach (TransitAccountProfile profile in profiles)
                {
                    inputAboutMe.Text = profile.AboutSelf;
                    break;
                }

                accountName.Text = string.Format("Hello, {0}!", Renderer.Render(SessionManager.Account.Name));
                accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", SessionManager.Account.PictureId);

                AccountNumbers numbers = (AccountNumbers)Cache[string.Format("accountnumbers:{0}", SessionManager.Ticket)];
                if (numbers == null)
                {
                    numbers = new AccountNumbers();
                    numbers.FirstDegreeCount = SocialService.GetFirstDegreeCountById(SessionManager.Account.Id);
                    numbers.SecondDegreeCount = SocialService.GetNDegreeCountById(SessionManager.Account.Id, 2);
                    numbers.AllCount = SocialService.GetAccountsCount();

                    DiscussionQueryOptions options = new DiscussionQueryOptions();
                    options.AccountId = SessionManager.Account.Id;
                    numbers.PostsCount = DiscussionService.GetUserDiscussionThreadsCount(options);

                    Cache.Insert(string.Format("accountnumbers:{0}", SessionManager.Ticket), 
                        numbers, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
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
                inputTimeZone.SelectedUtcOffset = SessionManager.Account.UtcOffset;

                ArrayList countries = new ArrayList();
                if (SessionManager.Account.Country.Length == 0) countries.Add(new TransitCountry());
                countries.AddRange(LocationService.GetCountries());

                ArrayList states = new ArrayList();
                if (SessionManager.Account.State.Length == 0) states.Add(new TransitState());
                states.AddRange(LocationService.GetStatesByCountry(SessionManager.Account.Country));

                inputCountry.DataSource = countries;
                inputCountry.DataBind();

                inputState.DataSource = states;
                inputState.DataBind();

                inputCountry.Items.FindByValue(SessionManager.Account.Country).Selected = true;
                inputState.Items.FindByValue(SessionManager.Account.State).Selected = true;

                inputSignature.Text = SessionManager.Account.Signature;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitAccount ta = new TransitAccount();
            ta.Birthday = inputBirthday.SelectedDate;
            ta.Name = inputName.Text;
            ta.City = inputCity.Text;
            ta.Country = inputCountry.SelectedValue;
            ta.State = inputState.SelectedValue;
            ta.UtcOffset = inputTimeZone.SelectedUtcOffset;
            ta.Signature = inputSignature.Text;

            if (ta.Signature.Length > inputSignature.MaxLength)
                throw new Exception(string.Format("Signature may not exceed {0} characters.", inputSignature.MaxLength));

            AccountService.UpdateAccount(SessionManager.Ticket, ta);
            Cache.Remove(string.Format("account:{0}", SessionManager.Ticket));
            ReportInfo("Account updated.");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void saveProfile_Click(object sender, EventArgs e)
    {
        try
        {
            List<TransitAccountProfile> profiles = AccountService.GetAccountProfiles(SessionManager.Ticket);
            TransitAccountProfile tp = 
                (profiles.Count > 0) ? profiles[0] : new TransitAccountProfile();
            tp.AboutSelf = inputAboutMe.Text;
            AccountService.CreateOrUpdateAccountProfile(SessionManager.Ticket, tp);
            ReportInfo("Account profile updated.");
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
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
