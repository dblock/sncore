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

public partial class EmailAccountPasswordReset : AuthenticatedPage
{
    private TransitAccount mAccount;

    public TransitAccount Account
    {
        get
        {
            try
            {
                if (mAccount == null)
                {
                    mAccount = SessionManager.AccountService.GetAccountById(RequestId);
                }
            }
            catch (Exception ex)
            {
                ReportException(ex);
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
}

