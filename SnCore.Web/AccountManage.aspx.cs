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

[SiteMapDataAttribute("Me Me")]
public partial class AccountManage : AuthenticatedPage
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
            if (!SessionManager.GetBool<TransitAccount, int>(
                SessionManager.AccountId, SessionManager.AccountService.HasVerifiedEmail))
            {
                noticeVerifiedEmail.HtmlEncode = false;
                noticeVerifiedEmail.Info = "You don't have a verified e-mail address. " +
                    "You will only be able to post once you have verified your e-mail. " +
                    "If you haven't received a confirmation e-mail, please " +
                    "<a href='AccountEmailsManage.aspx'>double-check your address</a>. " +
                    "Now is also a good time to <a href='AccountPicturesManage.aspx'>upload a picture</a>.";
            }
            else
            {
                IList<TransitAccountEmail> emails = SessionManager.GetCollection<TransitAccountEmail, int>(
                    SessionManager.AccountId, null, SessionManager.AccountService.GetAccountEmails);

                foreach (TransitAccountEmail email in emails)
                {
                    if (email.Failed)
                    {
                        noticeVerifiedEmail.HtmlEncode = false;
                        noticeVerifiedEmail.Info = string.Format("We tried to send you an e-mail to \"{0}\", which bounced with the following error: \"{1}\". " +
                            "Please <a href='AccountEmailsManage.aspx'>double-check your address</a>.", Renderer.Render(email.Address), Renderer.Render(email.LastError));
                        break;
                    }
                }
            }            

            accountName.Text = string.Format("Hello, {0}!", Renderer.Render(SessionManager.Account.Name));
            accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", SessionManager.Account.PictureId);

            groupsView.AccountId = SessionManager.AccountId;
            friendsView.AccountId = SessionManager.AccountId;

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

            accountFirstDegree.Visible = (numbers.FirstDegreeCount > 0);
            accountFirstDegree.Text = string.Format("{0} friend{1} in your personal network",
                numbers.FirstDegreeCount,
                numbers.FirstDegreeCount != 1 ? "s" : string.Empty);

            accountSecondDegree.Visible = (numbers.SecondDegreeCount > 0);
            accountSecondDegree.Text = string.Format("{0} friend{1} in your extended network",
                numbers.SecondDegreeCount,
                numbers.SecondDegreeCount != 1 ? "s" : string.Empty);

            accountAllDegrees.Text = string.Format("{0} {1} to make new friends with",
                numbers.NewCount > 0 ? numbers.NewCount.ToString() : "no",
                numbers.NewCount != 1 ? "people" : "person");

            accountDiscussionThreads.Visible = (numbers.PostsCount > 0);
            accountDiscussionThreads.Text = string.Format("{0} discussion post{1}",
                numbers.PostsCount, numbers.PostsCount != 1 ? "s" : string.Empty);
        }
    }
}
