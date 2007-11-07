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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;

public partial class AccountFriendRequestsSentManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        listSent.OnGetDataSource += new EventHandler(listSent_OnGetDataSource);
        if (!IsPostBack)
        {
            listSent.VirtualItemCount = SessionManager.SocialService.GetSentAccountFriendRequestsCount(
                SessionManager.Ticket, SessionManager.AccountId);

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Friend Requests", Request, "AccountFriendRequestsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Sent", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void listSent_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = listSent.CurrentPageIndex;
        options.PageSize = listSent.PageSize;
        listSent.DataSource = SessionManager.SocialService.GetSentAccountFriendRequests(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    public void GetData(object sender, EventArgs e)
    {
        listSent.CurrentPageIndex = 0;
        listSent_OnGetDataSource(sender, e);
        listSent.DataBind();
    }

    public void listSent_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Cancel":
                int id = int.Parse(e.CommandArgument.ToString());
                SessionManager.Delete<TransitAccountFriendRequest>(id, SessionManager.SocialService.DeleteAccountFriendRequest);
                GetData(sender, e);
                noticeManage.Info = "Request cancelled.";
                break;
        }
    }
}
