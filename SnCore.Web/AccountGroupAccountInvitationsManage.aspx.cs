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
using SnCore.Tools.Web;

public partial class AccountGroupAccountInvitationsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        listPending.OnGetDataSource += new EventHandler(listPending_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Invitations", Request.Url));
            StackSiteMap(sitemapdata);

            string action = Request["action"];
            if (!string.IsNullOrEmpty(action))
            {
                listPending_ItemCommand(sender, new DataListCommandEventArgs(null, null,
                    new CommandEventArgs(action, GetId("rid"))));
            }
        }
    }

    void listPending_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = listPending.CurrentPageIndex;
        options.PageSize = listPending.PageSize;
        listPending.DataSource = SessionManager.GroupService.GetAccountGroupAccountInvitationsByAccountId(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    public void GetData(object sender, EventArgs e)
    {
        listPending.CurrentPageIndex = 0;
        listPending.VirtualItemCount = SessionManager.GroupService.GetAccountGroupAccountInvitationsByAccountIdCount(
            SessionManager.Ticket, SessionManager.AccountId);
        listPending_OnGetDataSource(sender, e);
        listPending.DataBind();
        reasonTable.Visible = (listPending.Items.Count != 0);
    }

    public void listPending_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Accept":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    TransitAccountGroupAccountInvitation t_instance = SessionManager.GroupService.GetAccountGroupAccountInvitationById(
                        SessionManager.Ticket, id);
                    SessionManager.GroupService.AcceptAccountGroupAccountInvitation(SessionManager.Ticket, id, inputReason.Text);
                    GetData(sender, e);
                    ReportInfo(t_instance.AccountGroupIsPrivate && ! t_instance.RequesterIsAdministrator
                        ? "Since this is a private group, your membership must first be approved by the group administrator. A request has been submitted."
                        : "Invitation accepted.");
                    break;
                }
            case "Reject":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.GroupService.RejectAccountGroupAccountInvitation(SessionManager.Ticket, id, inputReason.Text);
                    GetData(sender, e);
                    ReportInfo("Invitation rejected.");
                    break;
                }
        }
    }
}
