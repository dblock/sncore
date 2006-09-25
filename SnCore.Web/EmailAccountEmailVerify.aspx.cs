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

public partial class EmailAccountEmailVerify : AuthenticatedPage
{
    private TransitAccountEmailConfirmation mAccountEmailConfirmation;

    public TransitAccountEmailConfirmation AccountEmailConfirmation
    {
        get
        {
            try
            {
                if (mAccountEmailConfirmation == null)
                {
                    mAccountEmailConfirmation = AccountService.GetAccountEmailConfirmationById(
                        SessionManager.Ticket, RequestId);
                }
            }
            catch (Exception ex)
            {
                ReportException(ex);
            }

            return mAccountEmailConfirmation;
        }
    }

    private TransitAccount mAccount;

    public TransitAccount Account
    {
        get
        {
            try
            {
                if (mAccount == null)
                {
                    mAccount = AccountService.GetAccountById(
                        AccountEmailConfirmation.AccountEmail.AccountId);
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

