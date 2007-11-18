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
        gridFriends.DataSource = SessionManager.GetCollection<TransitAccountAuditEntry, int>(
            AccountId, options, SessionManager.SocialService.GetAccountFriendAuditEntries);
    }

    private void GetData()
    {
        gridFriends.CurrentPageIndex = 0;
        gridFriends.VirtualItemCount = SessionManager.GetCount<TransitAccountAuditEntry, int>(
            AccountId, SessionManager.SocialService.GetAccountFriendAuditEntriesCount);
        gridFriends_OnGetDataSource(this, null);
        gridFriends.DataBind();
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridFriends.OnGetDataSource += new EventHandler(gridFriends_OnGetDataSource);
        gridFriends.RepeatRows = Max;

        if (!IsPostBack)
        {
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
