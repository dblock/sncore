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
                if (mAccountId == 0)
                {
                    mAccountId = SessionManager.Account.Id;
                }
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
                object[] args = { AccountId };
                mAccount = SessionManager.GetCachedItem<TransitAccount>(
                    AccountService, "GetAccountById", args);
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
                object[] args = { AccountId };
                mAccountPermissions = SessionManager.GetCachedItem<TransitAccountPermissions>(
                    AccountService, "GetAccountPermissionsById", args);
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

    public void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (RequestId == 0)
                {
                    counterProfileViews.Uri = string.Format("{0}?id={1}", Request.Url, AccountId);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);
            if (!IsPostBack)
            {
                linkDiscussionThreads.NavigateUrl = string.Format("AccountDiscussionThreadsView.aspx?id={0}", AccountId);
                linkDelete.NavigateUrl = string.Format("AccountDelete.aspx?id={0}", AccountId);
                linkResetPassword.NavigateUrl = string.Format("AccountChangePassword.aspx?id={0}", AccountId);
                linkAttributes.NavigateUrl = string.Format("AccountAttributesManage.aspx?id={0}", AccountId);

                attributesView.AccountId = AccountId;
                placeFavoritesView.AccountId = AccountId;
                placesView.AccountId = AccountId;
                propertygroupsView.AccountId = AccountId;
                friendsView.AccountId = AccountId;
                surveysView.AccountId = AccountId;
                websitesView.AccountId = AccountId;
                storiesView.AccountId = AccountId;
                feedsView.AccountId = AccountId;
                blogsView.AccountId = AccountId;
                licenseView.AccountId = AccountId;

                if (Account == null)
                {
                    ReportWarning("Account does not exist.");
                    pnlAccount.Visible = false;
                    return;
                }

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(Account.Name, Request.Url));
                StackSiteMap(sitemapdata);

                accountReminder.Visible = (RequestId == 0);

                this.Title = Renderer.Render(Account.Name);

                GetPicturesData(sender, e);
                
                accountLastLogin.Text = Adjust(Account.LastLogin).ToString("d");
                accountCity.Text = Renderer.Render(Account.City);
                accountState.Text = Renderer.Render(Account.State);
                accountCountry.Text = Renderer.Render(Account.Country);
                accountName.Text = Renderer.Render(Account.Name);
                accountId.Text = "#" + Account.Id.ToString();

                string returnurl = string.Format("AccountView.aspx?id={0}", Account.Id);

                linkNewMessage.NavigateUrl = string.Format("AccountMessageEdit.aspx?id={0}&ReturnUrl={1}&#edit",
                    Account.Id.ToString(), returnurl);

                linkAddToFriends.NavigateUrl = string.Format("AccountFriendRequestEdit.aspx?pid={0}&ReturnUrl={1}",
                    Account.Id.ToString(), returnurl);

                object[] args_aid = { Account.Id };
                discussionTags.DiscussionId = SessionManager.GetCachedCollectionCount(
                    DiscussionService, "GetTagDiscussionId", args_aid);

                linkLeaveTestimonial.NavigateUrl = string.Format("DiscussionPost.aspx?did={0}&ReturnUrl={1}&#edit",
                    discussionTags.DiscussionId, returnurl);

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

                panelAdmin.Visible = SessionManager.IsAdministrator && (AccountId != SessionManager.Account.Id);
                if (panelAdmin.Visible)
                {
                    linkPromoteAdmin.Visible = ! AccountPermissions.IsAdministrator;
                    linkDemoteAdmin.Visible = AccountPermissions.IsAdministrator;
                    linkDeleteFeatures.Visible = (LatestAccountFeature != null);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
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

    void GetPicturesData(object sender, EventArgs e)
    {
        AccountPicturesQueryOptions po = new AccountPicturesQueryOptions();
        po.Hidden = false;
        object[] p_args = { AccountId, po };
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            AccountService, "GetAccountPicturesCountById", p_args);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
        accountNoPicture.Visible = (picturesView.Items.Count == 0);
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            AccountPicturesQueryOptions po = new AccountPicturesQueryOptions();
            po.Hidden = false;
            ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
            object[] args = { AccountId, po, options };
            picturesView.DataSource = SessionManager.GetCachedCollection<TransitAccountPicture>(
                AccountService, "GetAccountPicturesById", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
