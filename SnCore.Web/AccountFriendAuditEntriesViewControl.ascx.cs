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

    public bool Friends
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "Friends", true);
        }
        set
        {
            ViewState["Friends"] = value;
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
        qopt.Friends = Friends;
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
            linkFriendsActivity.NavigateUrl = string.Format("AccountFriendAuditEntriesRss.aspx?Title={0}&Broadcast={1}&Friends={2}",
                Renderer.UrlEncode(Title), Broadcast, Friends);

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

    public void linkForward_Command(object sender, CommandEventArgs e)
    {
        TransitAccountAuditEntry t_instance = SessionManager.GetInstance<TransitAccountAuditEntry, int>(
            int.Parse(e.CommandArgument.ToString()), SessionManager.SocialService.GetAccountAuditEntryById);
        if (t_instance.IsBroadcast && t_instance.AccountId == SessionManager.AccountId)
            throw new Exception("You cannot forward your own broadcast.");
        TransitAccountAuditEntry t_forward = new TransitAccountAuditEntry();
        t_forward.AccountId = SessionManager.AccountId;
        t_forward.Description = string.Format("[user:{0}] forwarded: {1}", SessionManager.AccountId, t_instance.Description);
        t_forward.IsBroadcast = true;
        t_forward.IsPrivate = false;
        t_forward.IsSystem = false;
        t_forward.Url = t_instance.Url;
        SessionManager.CreateOrUpdate<TransitAccountAuditEntry>(
            t_forward, SessionManager.SocialService.CreateOrUpdateAccountAuditEntry);
        ReportInfo("Message has been successfully broadcasted to your friends!");
    }

    public void linkDelete_Command(object sender, CommandEventArgs e)
    {
        SessionManager.Delete<TransitAccountAuditEntry>(
            int.Parse(e.CommandArgument.ToString()), SessionManager.SocialService.DeleteAccountAuditEntry);
        gridFriends_OnGetDataSource(this, null);
        gridFriends.DataBind();
    }
}
