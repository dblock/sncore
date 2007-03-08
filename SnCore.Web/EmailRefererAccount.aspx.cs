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

public partial class EmailRefererAccount : AuthenticatedPage
{
    private TransitRefererAccount mRefererAccount;

    public TransitRefererAccount RefererAccount
    {
        get
        {
            if (mRefererAccount == null)
            {
                mRefererAccount = SessionManager.StatsService.GetRefererAccountById(
                    SessionManager.Ticket, RequestId);
            }
            return mRefererAccount;
        }
    }
}

