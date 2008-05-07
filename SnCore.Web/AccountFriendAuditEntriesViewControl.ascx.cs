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
using Wilco.Web.UI;
using Wilco.Web.UI.WebControls;
using SnCore.Services;
using System.Collections.Generic;
using SnCore.WebServices;
using SnCore.WebControls;

public partial class FriendAuditEntriesViewControl : Control
{
    public string Title
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "Title", "Audit Entries");
        }
        set
        {
            ViewState["Title"] = value;
        }
    }

    public bool Broadcast
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "Broadcast", false);
        }
        set
        {
            ViewState["Broadcast"] = value;
        }
    }

    public int OuterWidth
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "OuterWidth", 462);
        }
        set
        {
            ViewState["OuterWidth"] = value;
        }
    }

    public int Max
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "Max", 7);
        }
        set
        {
            ViewState["Max"] = value;
        }
    }

    void gridFriends_OnGetDataSource(object sender, EventArgs e)
    {
        if (AccountId <= 0)
            return;

        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridFriends.PageSize;
        options.PageNumber = gridFriends.CurrentPageIndex;
        gridFriends.DataSource = SessionManager.GetCollection<TransitAccountAuditEntry, TransitAccountAuditEntryQueryOptions>(
            GetQueryOptions(), options, SessionManager.SocialService.GetAccountFriendAuditEntries);
    }

    TransitAccountAuditEntryQueryOptions GetQueryOptions()
    {
        TransitAccountAuditEntryQueryOptions qopt = new TransitAccountAuditEntryQueryOptions();
        qopt.AccountId = AccountId;
        qopt.Broadcast = Broadcast;
        qopt.System = false;
        qopt.Private = false;
        return qopt;
    }

    private void GetData()
    {
        gridFriends.CurrentPageIndex = 0;
        gridFriends.VirtualItemCount = SessionManager.GetCount<TransitAccountAuditEntry, TransitAccountAuditEntryQueryOptions>(
            GetQueryOptions(), SessionManager.SocialService.GetAccountFriendAuditEntriesCount);
        gridFriends_OnGetDataSource(this, null);
        gridFriends.DataBind();
        this.Visible = (gridFriends.VirtualItemCount > 0);
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridFriends.OnGetDataSource += new EventHandler(gridFriends_OnGetDataSource);
        gridFriends.RepeatRows = Max;

        if (!IsPostBack)
        {
            linkFriendsActivity.NavigateUrl = string.Format("AccountFriendAuditEntriesRss.aspx?Title={0}&Broadcast={1}",
                Renderer.UrlEncode(Title), Broadcast);

            if (AccountId == 0)
                return;

            GetData();
        }
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
}
