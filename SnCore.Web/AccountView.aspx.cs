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

public partial class AccountView : Page
{
    private int mAccountId = -1;
    private TransitAccount mAccount = null;
    private TransitAccountPermissions mAccountPermissions = null;
    private TransitFeature mAccountFeature = null;

    public int AccountId
    {
        get
        {
            if (mAccountId < 0)
            {
                mAccountId = RequestId;
                if (mAccountId == 0) mAccountId = AccountService.GetAccountId(SessionManager.Ticket);
            }

            return mAccountId;
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = (TransitAccount)Cache[string.Format("account:{0}", AccountId)];
                if (mAccount == null)
                {
                    mAccount = AccountService.GetAccountById(AccountId);
                    Cache.Insert(string.Format("account:{0}", AccountId), 
                        mAccount, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }
            return mAccount;
        }
    }

    public TransitAccountPermissions AccountPermissions
    {
        get
        {
            if (mAccountPermissions == null)
            {
                mAccountPermissions = (TransitAccountPermissions)Cache[string.Format("accountpermissions:{0}", AccountId)];
                if (mAccountPermissions == null)
                {
                    mAccountPermissions = AccountService.GetAccountPermissionsById(AccountId);
                    Cache.Insert(string.Format("accountpermissions:{0}", AccountId),
                        mAccountPermissions, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }
            return mAccountPermissions;
        }
    }

    public TransitFeature LatestAccountFeature
    {
        get
        {
            if (mAccountFeature == null)
            {
                mAccountFeature = SystemService.FindLatestFeature(
                    "Account", Account.Id); 
            }
            return mAccountFeature;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                linkDiscussionThreads.NavigateUrl = string.Format("AccountDiscussionThreadsView.aspx?id={0}", AccountId);
                linkDelete.NavigateUrl = string.Format("AccountDelete.aspx?id={0}", AccountId);
                linkResetPassword.NavigateUrl = string.Format("AccountChangePassword.aspx?id={0}", AccountId);

                placeFavoritesView.AccountId = AccountId;
                placesView.AccountId = AccountId;
                profilesView.AccountId = AccountId;
                friendsView.AccountId = AccountId;
                surveysView.AccountId = AccountId;
                websitesView.AccountId = AccountId;
                storiesView.AccountId = AccountId;
                feedsView.AccountId = AccountId;
                blogsView.AccountId = AccountId;

                if (Account == null)
                {
                    ReportWarning("Account does not exist.");
                    pnlAccount.Visible = false;
                    return;
                }

                accountReminder.Visible = (RequestId == 0);

                this.Title = linkAccount.Text = Renderer.Render(Account.Name);

                picturesView.DataSource = AccountService.GetAccountPicturesById(Account.Id);
                picturesView.DataBind();

                if (picturesView.Items.Count == 0) accountNoPicture.Visible = true;

                accountLastLogin.Text = "last activity: " + Adjust(Account.LastLogin);
                accountCity.Text = Renderer.Render(Account.City);
                accountState.Text = Renderer.Render(Account.State);
                accountCountry.Text = Renderer.Render(Account.Country);
                accountName.Text = Renderer.Render(Account.Name);
                accountId.Text = "#" + Account.Id.ToString();
                linkNewMessage.NavigateUrl = string.Format("AccountMessageEdit.aspx?id={0}&ReturnUrl={1}&#edit",
                    Account.Id.ToString(),
                    Renderer.UrlEncode(string.Format("AccountView.aspx?id={0}", Account.Id)));
                linkAddToFriends.NavigateUrl = string.Format("AccountFriendRequestEdit.aspx?pid={0}&ReturnUrl={1}",
                    Account.Id.ToString(),
                    Renderer.UrlEncode(string.Format("AccountView.aspx?id={0}", Account.Id)));

                discussionTags.DiscussionId = DiscussionService.GetTagDiscussionId(Account.Id);

                websitesView.DataBind();
                surveysView.DataBind();
                storiesView.DataBind();
                discussionTags.DataBind();

                if (SessionManager.IsAdministrator)
                {
                    linkFeature.Text = (LatestAccountFeature != null)
                        ? string.Format("Feature &#187; Last on {0}", Adjust(LatestAccountFeature.Created).ToString("d"))
                        : "Feature &#187; Never Featured";
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        panelAdmin.Visible = SessionManager.IsAdministrator && (AccountId != SessionManager.Account.Id);
        if (panelAdmin.Visible)
        {
            linkPromoteAdmin.Visible = !AccountPermissions.IsAdministrator;
            linkDemoteAdmin.Visible = AccountPermissions.IsAdministrator;
            linkDeleteFeatures.Visible = (LatestAccountFeature != null);
        }
        base.OnPreRender(e);
    }

    public void promoteAdmin_Click(object sender, EventArgs e)
    {
        try
        {
            if (RequestId == 0)
            {
                throw new Exception("You cannot make yourself administrator.");
            }

            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to promote other users.");
            }

            AccountService.PromoteAdministrator(SessionManager.Ticket, AccountId);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void feature_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to feature other users.");
            }

            TransitFeature t_feature = new TransitFeature();
            t_feature.DataObjectName = "Account";
            t_feature.DataRowId = AccountId;
            SystemService.CreateOrUpdateFeature(SessionManager.Ticket, t_feature);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void demoteAdmin_Click(object sender, EventArgs e)
    {
        try
        {
            if (RequestId == 0)
            {
                throw new Exception("You cannot make yourself administrator.");
            }

            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to demote other users.");
            }

            AccountService.DemoteAdministrator(SessionManager.Ticket, AccountId);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }


    public void impersonate_Click(object sender, EventArgs e)
    {
        try
        {
            if (RequestId == 0)
            {
                throw new Exception("You cannot impersonate self.");
            }

            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to impersonate users.");
            }

            if (SessionManager.IsImpersonating)
            {
                throw new Exception("You're already impersonating a user.");
            }

            SessionManager.Impersonate(AccountService.Impersonate(SessionManager.Ticket, AccountId));
            Response.Redirect("AccountView.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to feature accounts.");
            }

            TransitFeature t_feature = new TransitFeature();
            t_feature.DataObjectName = "Account";
            t_feature.DataRowId = RequestId;
            SystemService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
