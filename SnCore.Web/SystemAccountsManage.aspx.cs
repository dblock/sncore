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
using SnCore.Services;

public partial class SystemAccountsManage : AuthenticatedPage
{
    public void Page_Load()
    {
        SetDefaultButton(linkLookup);

        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Accounts", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void linkLookup_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(inputEmail.Text))
        {
            throw new Exception("Missing E-Mail");
        }

        gridLookup.DataSource = SessionManager.AccountService.FindAllByEmail(
            SessionManager.Ticket, inputEmail.Text, null);
        gridLookup.DataBind();
    }

    public void gridLookup_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Impersonate":

                int id = int.Parse(e.CommandArgument.ToString());

                if (SessionManager.AccountId == id)
                {
                    throw new Exception("You cannot impersonate self.");
                }

                if (!SessionManager.IsAdministrator)
                {
                    // avoid round-trip
                    throw new Exception("You must be an administrator to impersonate users.");
                }

                if (SessionManager.IsImpersonating)
                {
                    throw new Exception("You're already impersonating a user.");
                }

                SessionManager.Impersonate(SessionManager.AccountService.Impersonate(
                    SessionManager.Ticket, id));
                Response.Redirect("AccountView.aspx");

                break;
        }
    }
}
