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
using SnCore.SiteMap;

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
                    mAccount = SessionManager.AccountService.GetAccountById(
                        SessionManager.Ticket, RequestId);
                }
            }
            return mAccount;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", Account.PictureId);
            accountName.Text = string.Format("Dear {0},", Renderer.Render(Account.Name));

            inputConfirm.Checked = SessionManager.IsAdministrator;
            inputDeleteContent.Enabled = SessionManager.IsAdministrator;
            inputDeleteContent.Checked = SessionManager.IsAdministrator;

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Delete Account", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(buttonDelete);
    }

    public void delete_Click(object sender, EventArgs e)
    {
        if (! inputConfirm.Checked)
        {
            throw new Exception("Please check the \"I understand that this cannot be undone.\" box.");
        }

        TransitAccountDeleteOptions options = new TransitAccountDeleteOptions();
        options.DeleteContent = inputDeleteContent.Checked && SessionManager.IsAdministrator;
        SessionManager.AccountService.DeleteAccountWithOptions(SessionManager.Ticket, Account.Id, options);
        SessionManager.FlushCache();
        pnlAccount.Visible = false;

        if (!SessionManager.IsAdministrator)
        {
            // logout the user that deletes himself only
            SessionManager.Logout();
        }

        ReportInfo("Account deleted.");
    }
}
