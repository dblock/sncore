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
using SnCore.SiteMap;

public partial class AccountView : Page
{
    private int mAccountId = -1;
    private AccountService.TransitAccount mAccount = null;

    public int AccountId
    {
        get
        {
            if (mAccountId < 0)
            {
                mAccountId = RequestId;
                if (mAccountId == 0)
                {
                    mAccountId = SessionManager.Account.Id;
                }
            }

            return mAccountId;
        }
    }

    public AccountService.TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = SessionManager.GetInstance<AccountService.TransitAccount, AccountService.ServiceQueryOptions, int>(
                    AccountId, SessionManager.AccountService.GetAccountById);
            }
            return mAccount;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Account == null)
            {
                throw new Exception("Account does not exist.");
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(Account.Name, Request.Url));
            StackSiteMap(sitemapdata);

            this.Title = Renderer.Render(Account.Name);

            accountLastLogin.Text = string.Format("last activity: {0}",
                SessionManager.Adjust(Account.LastLogin).ToString("d"));

            accountCity.Text = Renderer.Render(Account.City);
            accountState.Text = Renderer.Render(Account.State);
            accountCountry.Text = Renderer.Render(Account.Country);
            accountName.Text = Renderer.Render(Account.Name);
            accountId.Text = string.Format("account id: #{0}", Account.Id);
        }
    }
}
