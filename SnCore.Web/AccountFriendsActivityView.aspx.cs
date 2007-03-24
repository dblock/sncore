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

public partial class AccountFriendsActivityView : AccountPersonPage
{
    public int RequestAccountId
    {
        get
        {
            return RequestId > 0 ? RequestId : SessionManager.Account.Id;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridFriends.OnGetDataSource += new EventHandler(gridFriends_OnGetDataSource);

        if (!IsPostBack)
        {
            TransitAccount ta = SessionManager.GetInstance<TransitAccount, int>(
                RequestAccountId, SessionManager.AccountService.GetAccountById);

            labelName.Text = string.Format("{0}'s Friends Activity", Render(ta.Name));

            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Friends Activity", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    private void GetData()
    {
        gridFriends.CurrentPageIndex = 0;
        gridFriends.VirtualItemCount = SessionManager.GetCount<TransitAccountActivity, int>(
            AccountId, SessionManager.SocialService.GetFriendsAccountActivityCount);
        gridFriends_OnGetDataSource(this, null);
        gridFriends.DataBind();
    }

    void gridFriends_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridFriends.PageSize;
        options.PageNumber = gridFriends.CurrentPageIndex;
        gridFriends.DataSource = SessionManager.GetCollection<TransitAccountActivity, int>(
            AccountId, options, SessionManager.SocialService.GetFriendsAccountActivity);
        panelFriends.Update();
    }
}
