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

public partial class AccountGroupAccountRequestsManage : AuthenticatedPage
{
    public int GroupId
    {
        get
        {
            return RequestId;
        }
    }

    private TransitAccountGroup mAccountGroup;

    public TransitAccountGroup AccountGroup
    {
        get
        {
            if (mAccountGroup == null)
            {
                mAccountGroup = SessionManager.GroupService.GetAccountGroupById(
                    SessionManager.Ticket, GroupId);
            }
            return mAccountGroup;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        listPending.OnGetDataSource += new EventHandler(listPending_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            labelGroupName.Text = Renderer.Render(AccountGroup.Name);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(AccountGroup.Name, Request, string.Format("AccountGroupView.aspx?id={0}", GroupId)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Membership Requests", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void listPending_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = listPending.CurrentPageIndex;
        options.PageSize = listPending.PageSize;
        listPending.DataSource = SessionManager.GroupService.GetAccountGroupAccountRequests(
            SessionManager.Ticket, GroupId, options);
    }

    public void GetData(object sender, EventArgs e)
    {
        listPending.CurrentPageIndex = 0;
        listPending.VirtualItemCount = SessionManager.GroupService.GetAccountGroupAccountRequestsCount(
            SessionManager.Ticket, GroupId);
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
                    SessionManager.GroupService.AcceptAccountGroupAccountRequest(SessionManager.Ticket, id, inputReason.Text);
                    GetData(sender, e);
                    ReportInfo("Request accepted.");
                    break;
                }
            case "Reject":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.GroupService.RejectAccountGroupAccountRequest(SessionManager.Ticket, id, inputReason.Text);
                    GetData(sender, e);
                    ReportInfo("Request rejected.");
                    break;
                }
        }
    }
}
