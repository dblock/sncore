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
                    mAccount = SessionManager.AccountService.GetAccountById(RequestId);
                }
            }
            return mAccount;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", Account.PictureId);
                accountName.Text = string.Format("Dear {0},", Renderer.Render(Account.Name));

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Delete Account", Request.Url));
                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(buttonDelete);
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
            SessionManager.AccountService.DeleteAccountById(SessionManager.Ticket, Account.Id, inputPassword.Text);
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
