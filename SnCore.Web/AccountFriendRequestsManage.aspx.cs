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

public partial class AccountFriendRequestsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            listPending.OnGetDataSource += new EventHandler(listPending_OnGetDataSource);
            
            if (!IsPostBack)
            {
                GetData(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Friend Requests", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void listPending_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = listPending.CurrentPageIndex;
        options.PageSize = listPending.PageSize;
        listPending.DataSource = SocialService.GetAccountFriendRequests(SessionManager.Ticket, options);
    }

    public void GetData(object sender, EventArgs e)
    {
        listPending.CurrentPageIndex = 0;
        listPending.VirtualItemCount = SocialService.GetAccountFriendRequestsCount(SessionManager.Ticket);
        listPending_OnGetDataSource(sender, e);
        listPending.DataBind();

        if (listPending.Items.Count == 0)
        {
            noticeManage.Info = "You don't have any pending requests.";
        }
    }

    public void listPending_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Accept":
                    {
                        int id = int.Parse(e.CommandArgument.ToString());
                        SocialService.AcceptAccountFriendRequest(SessionManager.Ticket, id, inputReason.Text);
                        GetData(sender, e);
                        noticeManage.Info = "Friend request accepted.";
                        break;
                    }
                case "Reject":
                    {
                        int id = int.Parse(e.CommandArgument.ToString());
                        SocialService.RejectAccountFriendRequest(SessionManager.Ticket, id, inputReason.Text);
                        GetData(sender, e);
                        noticeManage.Info = "Friend request rejected.";
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
