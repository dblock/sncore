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
using SnCore.SiteMap;

[SiteMapDataAttribute("Welcome")]
public partial class AccountCreateWelcome : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            labelAccountName.Text = Render(SessionManager.Account.Name);
            panelEmailConfirmed.Visible = SessionManager.GetBool<TransitAccount, int>(
                SessionManager.AccountId, SessionManager.AccountService.HasVerifiedEmail);
            panelEmailNotConfirmed.Visible = ! panelEmailConfirmed.Visible;
        }
    }
}
