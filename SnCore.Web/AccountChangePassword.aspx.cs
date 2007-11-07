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
using SnCore.SiteMap;
using SnCore.Services;

public partial class AccountChangePassword : AuthenticatedPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
        sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
        sitemapdata.Add(new SiteMapDataAttributeNode("Security", "AccountPreferencesManage.aspx#security"));
        sitemapdata.Add(new SiteMapDataAttributeNode("Change Password", Request.Url));
        StackSiteMap(sitemapdata);

        if (!string.IsNullOrEmpty(PasswordHash))
        {
            int account_id = (RequestId > 0) ? RequestId : SessionManager.Account.Id;

            if (!SessionManager.AccountService.IsPasswordValidMd5(
                SessionManager.Ticket,
                account_id,
                PasswordHash))
            {
                Redirect(string.Format("AccountChangePassword.aspx?ReturnUrl={0}", 
                    Renderer.UrlEncode(ReturnUrl)));
                return;
            }

            panelOldPassword.Visible = false;
        }

        if (SessionManager.Account.IsPasswordExpired)
        {
            ReportInfo("Your password has expired. Please change your password.");
        }

        SetDefaultButton(manageAccountChangePassword);
    }

    public string PasswordHash
    {
        get
        {
            object hash = Request.QueryString["PasswordHash"];
            if (hash == null) return string.Empty;
            return hash.ToString();
        }
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? string.Empty : o.ToString());
        }
    }

    protected void changePassword_Click(object sender, EventArgs e)
    {
        try
        {
            if (inputNewPassword.Text != inputNewPassword2.Text)
            {
                throw new ArgumentException("Passwords don't match.");
            }

            int account_id = (RequestId > 0) ? RequestId : SessionManager.Account.Id;

            if (!string.IsNullOrEmpty(PasswordHash))
            {
                SessionManager.AccountService.ChangePasswordMd5(
                    SessionManager.Ticket,
                    account_id,
                    PasswordHash,
                    inputNewPassword.Text);
            }
            else
            {
                SessionManager.AccountService.ChangePassword(
                    SessionManager.Ticket,
                    account_id,
                    inputOldPassword.Text,
                    inputNewPassword.Text);
            }

            ReportInfo("Password changed.");
            panelChangePassword.Visible = false;

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                Redirect(ReturnUrl);
            }
        }
        catch
        {
            inputOldPassword.Attributes["value"] = inputOldPassword.Text;
            inputNewPassword.Attributes["value"] = inputNewPassword.Text;
            inputNewPassword2.Attributes["value"] = inputNewPassword2.Text;
            throw;
        }
    }

}
