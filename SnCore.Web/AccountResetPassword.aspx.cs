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
using SnCore.SiteMap;
using System.Collections.Generic;
using SnCore.Services;

public partial class AccountResetPassword : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Security", "AccountPreferencesManage.aspx#security"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Reset Password", Request.Url));
            StackSiteMap(sitemapdata);

            linkAdministrator.OnClientClick =
                string.Format("location.href='mailto:{0}';",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Admin.EmailAddress", "admin@localhost.com"));

            if (SessionManager.IsLoggedIn)
            {
                if (!SessionManager.HasVerified())
                {
                    ReportWarning("You don't have any verified e-mail addresses and/or profile photos.\n" +
                        "You must add/confirm a valid e-mail address and upload a profile photo before resetting your password.");

                    panelReset.Enabled = false;
                }

                string email = SessionManager.AccountService.GetActiveEmailAddress(
                    SessionManager.Ticket, SessionManager.AccountId);

                resetpasswordEmailAddress.Text = email;
                resetpasswordBirthday.SelectedDate = SessionManager.Account.Birthday;
            }
        }

        SetDefaultButton(resetPassword);

    }

    protected void resetPassword_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(resetpasswordEmailAddress.Text))
        {
            throw new Exception("Please enter your e-mail address.");
        }

        if (!resetpasswordBirthday.HasDate)
        {
            throw new Exception("Please select a valid date.");
        }

        SessionManager.AccountService.ResetPassword(resetpasswordEmailAddress.Text, resetpasswordBirthday.SelectedDate);
        ReportInfo("A new password was sent to '" + resetpasswordEmailAddress.Text + "'. Click <a href='AccountLogin.aspx'>here</a> to login.");
        panelReset.Visible = false;
        panelResetUpdate.Update();
    }

}
