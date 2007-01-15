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

public partial class SystemInvitationsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            gridManage_OnGetDataSource(this, null);
            gridManage.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Invitations", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.AccountService.GetAccountInvitations(
            SessionManager.Ticket, SessionManager.AccountId, null);
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                switch (e.CommandName)
                {
                    case "Delete":
                        SessionManager.AccountService.DeleteAccountInvitation(SessionManager.Ticket, id);
                        ReportInfo("Invitation deleted.");
                        gridManage.CurrentPageIndex = 0;
                        gridManage_OnGetDataSource(source, e);
                        gridManage.DataBind();
                        break;
                }
                break;
        }
    }

}
