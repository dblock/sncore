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

public partial class AccountFriendsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        friendsList.OnGetDataSource += new EventHandler(friendsList_OnGetDataSource);

        if (!IsPostBack)
        {
            friendsList.VirtualItemCount = SessionManager.SocialService.GetAccountFriendsCount(
                SessionManager.Ticket, SessionManager.AccountId);
            friendsList_OnGetDataSource(this, null);
            friendsList.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Friends", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void friendsList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = friendsList.CurrentPageIndex;
        options.PageSize = friendsList.PageSize;
        friendsList.DataSource = SessionManager.SocialService.GetAccountFriends(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    public void friendsList_Command(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                SessionManager.SocialService.DeleteAccountFriend(SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                friendsList.CurrentPageIndex = 0;
                friendsList_OnGetDataSource(sender, e);
                friendsList.DataBind();
                ReportInfo("Friend deleted.");
                break;
        }
    }
}
