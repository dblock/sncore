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

            GetPicturesData(sender, e);

            TransitAccountGroupAccount t_account = GetAccountGroupAccount();

            // links
            linkRequests.NavigateUrl = string.Format("AccountGroupAccountRequestsManage.aspx?id={0}", AccountGroupId);
            linkRequests.Visible = AccountGroup.IsPrivate &&
                ((t_account != null && t_account.IsAdministrator) || SessionManager.IsAdministrator);
            linkRequest.Visible = (t_account == null);

            // text
            accountgroupName.Text = Renderer.Render(AccountGroup.Name);
            accountgroupDescription.Text = Renderer.Render(AccountGroup.Description);
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
        if (!SessionManager.IsLoggedIn)
        {
            RedirectToLogin();
            return;
        }

        if (AccountGroup.IsPrivate)
        {
            Redirect(string.Format("AccountGroupAccountRequestEdit.aspx?gid={0}", AccountGroupId));
        }
        else
        {
            TransitAccountGroupAccount t_instance = new TransitAccountGroupAccount();
            t_instance.AccountId = SessionManager.AccountId;
            t_instance.AccountGroupId = AccountGroupId;
            t_instance.IsAdministrator = false;
            SessionManager.GroupService.CreateOrUpdateAccountGroupAccount(SessionManager.Ticket, t_instance);
            ReportInfo(string.Format("Welcome to \"{0}\"!", Renderer.Render(AccountGroup.Name)));
        }
    }
}
