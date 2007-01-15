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
using System.Collections.Generic;

public partial class AccountsActiveViewControl : Control
{
    private int mCount = 2;

    public int Count
    {
        get
        {
            return mCount;
        }
        set
        {
            mCount = value;
        }
    }

    public void Page_Load()
    {
        if (!IsPostBack)
        {
            object[] args = { SessionManager.Ticket, Count };
            accounts.DataSource = SessionManager.GetCachedCollection<TransitAccount>(
                SessionManager.SocialService, "GetActiveAccounts", args);
            accounts.RepeatColumns = Count;
            accounts.DataBind();
        }
    }
}
