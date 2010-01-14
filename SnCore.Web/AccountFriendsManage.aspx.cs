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
    private TransitAccountFriendQueryOptions GetOptions()
    {
        TransitAccountFriendQueryOptions options = new TransitAccountFriendQueryOptions();
        options.AccountId = SessionManager.AccountId;
        options.Name = searchFriends.Text;
        return options;
    }

    public void Page_Load(object sender, EventArgs e)
    {
        friendsList.OnGetDataSource += new EventHandler(friendsList_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Friends", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        friendsList.CurrentPageIndex = 0;
        friendsList.VirtualItemCount = SessionManager.SocialService.GetAccountFriendsCount(
            SessionManager.Ticket, GetOptions());
        friendsList_OnGetDataSource(this, null);
        friendsList.DataBind();
    }

    void friendsList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = friendsList.CurrentPageIndex;
        options.PageSize = friendsList.PageSize;
        friendsList.DataSource = SessionManager.SocialService.GetAccountFriends(
            SessionManager.Ticket, GetOptions(), options);
    }

    public void friendsList_Command(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.CommandArgument.ToString());
                SessionManager.Delete<TransitAccountFriend>(id, SessionManager.SocialService.DeleteAccountFriend);
                GetData(sender, e);
                ReportInfo("Friend deleted.");
                break;
        }
    }

    public void searchFriends_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
    }
}
