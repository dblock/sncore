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
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountDelete : AuthenticatedPage
{
    private TransitAccount mAccount = null;

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                if (RequestId == 0)
                {
                    mAccount = SessionManager.Account;
                }
                else
                {
                    mAccount = AccountService.GetAccountById(RequestId);
                }
            }
            return mAccount;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(buttonDelete);
            if (!IsPostBack)
            {
                accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", Account.PictureId);
                accountName.Text = string.Format("Dear {0},", Renderer.Render(Account.Name));
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void delete_Click(object sender, EventArgs e)
    {
        try
        {
            AccountService.DeleteAccountById(SessionManager.Ticket, Account.Id, inputPassword.Text);
            pnlAccount.Visible = false;
            SessionManager.Logout();
            ReportInfo("Account deleted.");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
