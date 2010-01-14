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
using Wilco.Web.UI;
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountFriendsViewControl : Control
{
    private TransitAccountFriendQueryOptions GetOptions()
    {
        TransitAccountFriendQueryOptions options = new TransitAccountFriendQueryOptions();
        options.AccountId = AccountId;
        return options;
    }

    public int AccountId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountId", 0);
        }
        set
        {
            ViewState["AccountId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        friendsList.OnGetDataSource += new EventHandler(friendsList_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            linkAll.Text = string.Format("&#187; {0} Friend{1}",
                friendsList.VirtualItemCount, friendsList.VirtualItemCount == 1 ? string.Empty : "s");
            linkAll.NavigateUrl = string.Format("AccountFriendsView.aspx?id={0}", AccountId);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        friendsList.CurrentPageIndex = 0;
        friendsList.VirtualItemCount = SessionManager.GetCount<TransitAccountFriend, TransitAccountFriendQueryOptions>(
            GetOptions(), SessionManager.SocialService.GetAccountFriendsCount);
        friendsList_OnGetDataSource(sender, e);
        friendsList.DataBind();
        this.Visible = (friendsList.VirtualItemCount > 0);
    }

    void friendsList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = friendsList.CurrentPageIndex;
        options.PageSize = friendsList.PageSize;
        friendsList.DataSource = SessionManager.GetCollection<TransitAccountFriend, TransitAccountFriendQueryOptions>(
            GetOptions(), options, SessionManager.SocialService.GetAccountFriends);
        panelGrid.Update();
    }
}
