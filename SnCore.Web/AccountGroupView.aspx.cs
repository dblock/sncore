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

public partial class AccountGroupView : Page
{
    private TransitAccountGroup mAccountGroup = null;
    private TransitFeature mAccountGroupFeature = null;

    public int AccountGroupId
    {
        get
        {
            return RequestId;
        }
    }

    public TransitAccountGroup AccountGroup
    {
        get
        {
            if (mAccountGroup == null)
            {
                mAccountGroup = SessionManager.GetInstance<TransitAccountGroup, int>(
                    AccountGroupId, SessionManager.GroupService.GetAccountGroupById);
            }
            return mAccountGroup;
        }
    }

    private TransitAccountGroupAccount GetAccountGroupAccount()
    {
        try
        {
            if (!SessionManager.IsLoggedIn)
                return null;

            return SessionManager.GroupService.GetAccountGroupAccountByAccountGroupId(
                SessionManager.Ticket, SessionManager.AccountId, AccountGroupId);
        }
        catch
        {
        }

        return null;
    }

    public void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (RequestId == 0)
            {
                counterAccountGroupViews.Uri = string.Format("{0}?id={1}", Request.Url, AccountGroupId);
            }
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);
        if (!IsPostBack)
        {
            if (AccountGroup == null)
            {
                ReportWarning("Group does not exist.");
                pnlAccountGroup.Visible = false;
                return;
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(AccountGroup.Name, Request.Url));
            StackSiteMap(sitemapdata);

            this.Title = Renderer.Render(AccountGroup.Name);

            TransitAccountGroupAccount t_account = GetAccountGroupAccount();

            bool fGroupAdmin = ((t_account != null && t_account.IsAdministrator) || SessionManager.IsAdministrator);
            bool fGroupMember = (t_account != null);
            bool fGroupMemberOrAdmin = (fGroupMember || SessionManager.IsAdministrator);

            // links
            linkRequests.NavigateUrl = string.Format("AccountGroupAccountRequestsManage.aspx?id={0}", AccountGroupId);
            linkRequests.Visible = AccountGroup.IsPrivate && fGroupAdmin;
            linkPictures.NavigateUrl = string.Format("AccountGroupPicturesManage.aspx?id={0}", AccountGroupId);
            linkPictures.Visible = fGroupMemberOrAdmin;
            linkRequest.Visible = ! fGroupMember;
            linkLeave.Visible = fGroupMember;
            linkDelete.Visible = fGroupAdmin;
            discussionView.Visible = fGroupMemberOrAdmin;
            linkInviteFriend.NavigateUrl = string.Format("AccountGroupAccountInvitationEdit.aspx?gid={0}", AccountGroupId);
            linkInviteFriend.Visible = fGroupMemberOrAdmin;

            linkEditGroup.NavigateUrl = string.Format("AccountGroupEdit.aspx?id={0}&ReturnUrl={1}", AccountGroupId,
                Renderer.UrlEncode(Request.Url.PathAndQuery));
            linkEditGroupDiscussion.NavigateUrl = string.Format("SystemDiscussionEdit.aspx?id={0}&ReturnUrl={1}",
                SessionManager.DiscussionService.GetOrCreateDiscussionId(SessionManager.Ticket, "AccountGroup", AccountGroupId),
                Renderer.UrlEncode(Request.Url.PathAndQuery));
            
            panelGroupAdmin.Visible = fGroupAdmin;

            // text
            accountgroupName.Text = Renderer.Render(AccountGroup.Name);
            accountgroupDescription.Text = Renderer.RenderEx(AccountGroup.Description);

            // action
            string action = Request["action"];
            if (! string.IsNullOrEmpty(action))
            {
                switch (action)
                {
                    case "join":
                        linkRequest_Click(sender, e);
                        break;
                    case "leave":
                        linkLeave_Click(sender, e);
                        break;
                }
            }

            // feature
            if (SessionManager.IsAdministrator)
            {
                linkFeature.Text = (LatestAccountGroupFeature != null)
                    ? string.Format("Feature &#187; Last on {0}", Adjust(LatestAccountGroupFeature.Created).ToString("d"))
                    : "Feature &#187; Never Featured";
            }

            // private / public
            if (AccountGroup.IsPrivate && ! fGroupMemberOrAdmin)
            {
                if (!SessionManager.IsLoggedIn)
                {
                    RedirectToLogin();
                }
                else
                {
                    ReportWarning("This is a private group. You must be a member to see it.");
                }

                return; 
            }

            GetPicturesData(sender, e);

            accountsView.AccountGroupId = AccountGroupId;

            if (fGroupMemberOrAdmin)
            {
                placesView.AccountGroupId = AccountGroupId;
                discussionView.DiscussionId = SessionManager.DiscussionService.GetOrCreateDiscussionId(
                    SessionManager.Ticket, "AccountGroup", RequestId);
            }

            linkRelRss.NavigateUrl = string.Format("AccountGroupRss.aspx?id={0}", AccountGroupId);
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCount<TransitAccountGroupPicture, int>(
            AccountGroupId, SessionManager.GroupService.GetAccountGroupPicturesCount);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
        accountNoPicture.Visible = (picturesView.Items.Count == 0);
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
        picturesView.DataSource = SessionManager.GetCollection<TransitAccountGroupPicture, int>(
            AccountGroupId, options, SessionManager.GroupService.GetAccountGroupPictures);
    }

    public void linkRequest_Click(object sender, EventArgs e)
    {
        string requesturi = string.Format("AccountGroupAccountRequestEdit.aspx?gid={0}", AccountGroupId);

        if (!SessionManager.IsLoggedIn)
        {
            RedirectToLogin(requesturi);
            return;
        }

        if (AccountGroup.IsPrivate && ! SessionManager.IsAdministrator)
        {
            Redirect(requesturi);
        }
        else
        {
            TransitAccountGroupAccount t_instance = new TransitAccountGroupAccount();
            t_instance.AccountId = SessionManager.AccountId;
            t_instance.AccountGroupId = AccountGroupId;
            t_instance.IsAdministrator = false;
            SessionManager.GroupService.CreateOrUpdateAccountGroupAccount(SessionManager.Ticket, t_instance);
            Redirect(string.Format("AccountGroupView.aspx?id={0}", AccountGroupId));
        }
    }

    public void linkLeave_Click(object sender, EventArgs e)
    {
        TransitAccountGroupAccount t_account = GetAccountGroupAccount();
        SessionManager.GroupService.DeleteAccountGroupAccount(SessionManager.Ticket, t_account.Id);
        Redirect(string.Format("AccountGroupView.aspx?id={0}", AccountGroupId));
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        SessionManager.Delete<TransitAccountGroup>(AccountGroupId, SessionManager.GroupService.DeleteAccountGroup);
        Redirect("AccountGroupsView.aspx");
    }

    protected override void OnPreRender(EventArgs e)
    {
        panelAdmin.Visible = SessionManager.IsAdministrator;
        linkDeleteFeatures.Visible = (LatestAccountGroupFeature != null);
        base.OnPreRender(e);
    }

    public void feature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature groups.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountGroup";
        t_feature.DataRowId = RequestId;
        SessionManager.CreateOrUpdate<TransitFeature>(
            t_feature, SessionManager.ObjectService.CreateOrUpdateFeature);
        Redirect(Request.Url.PathAndQuery);
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature AccountGroups.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountGroup";
        t_feature.DataRowId = RequestId;
        SessionManager.ObjectService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
        SessionManager.InvalidateCache<TransitFeature>();
        Redirect(Request.Url.PathAndQuery);
    }

    public TransitFeature LatestAccountGroupFeature
    {
        get
        {
            if (mAccountGroupFeature == null)
            {
                mAccountGroupFeature = SessionManager.GetInstance<TransitFeature, string, int>(
                    "AccountGroup", RequestId, SessionManager.ObjectService.FindLatestFeature);
            }
            return mAccountGroupFeature;
        }
    }
}
