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
using SnCore.Tools.Web;

/// <summary>
/// This e-mail template should be used with a Reminder:
/// 
/// Once-a-month
/// Account
/// Last Login
/// 
/// </summary>
public partial class EmailAccountLoginReminder : AuthenticatedPage
{
    private TransitAccount mAccount;

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null && RequestId > 0)
            {
                mAccount = SessionManager.AccountService.GetAccountById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccount;
        }
    }

    public string MailtoAdministrator
    {
        get
        {
            return string.Format("mailto:{0}", Render(SessionManager.GetCachedConfiguration(
                "SnCore.Admin.EmailAddress", "admin@localhost.com")));
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("{0} Misses You",
            Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")));
    }
}

