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
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;

public partial class AccountGroupRss : AccountPersonPage
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

    public string RssTitle
    {
        get
        {
            return Renderer.Render(string.Format("{0}: {1}",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"),
                Renderer.Render(AccountGroup.Name)));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 10;
            // get new members
            rssRepeaterMembers.DataSource = SessionManager.GetCollection<TransitAccountGroupAccount, int>(
                AccountGroupId, options, SessionManager.GroupService.GetAccountGroupAccounts);
            rssRepeaterMembers.DataBind();
            // get new places
            rssRepeaterPlaces.DataSource = SessionManager.GetCollection<TransitAccountGroupPlace, int>(
                AccountGroupId, options, SessionManager.GroupService.GetAccountGroupPlaces);
            rssRepeaterPlaces.DataBind();
            // get new discussion posts
            int discussion_id = SessionManager.DiscussionService.GetOrCreateDiscussionId(
                    SessionManager.Ticket, "AccountGroup", AccountGroupId);
            rssRepeaterDiscussion.DataSource = SessionManager.GetCollection<TransitDiscussionPost, int>(
                discussion_id, options, SessionManager.DiscussionService.GetLatestDiscussionPostsById);
            rssRepeaterDiscussion.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}
