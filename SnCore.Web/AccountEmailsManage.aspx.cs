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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountEmailsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("E-Mails", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.AccountService.GetAccountEmailsCount(
            SessionManager.Ticket, SessionManager.AccountId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.AccountService.GetAccountEmails(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }


    private enum Cells
    {
        id = 0,
        address,
        verified,
        principal,
        image,
        addressview,
        delete,
        resend,
        setprincipal
    };

    public void gridManage_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                bool verified = bool.Parse(e.Item.Cells[(int)Cells.verified].Text);
                string address = e.Item.Cells[(int)Cells.address].Text;
                bool principal = bool.Parse(e.Item.Cells[(int)Cells.principal].Text);

                if (verified)
                {
                    e.Item.Cells[(int)Cells.resend].Text = string.Empty;
                }

                if (principal || !verified)
                {
                    e.Item.Cells[(int)Cells.setprincipal].Text = string.Empty;
                }

                break;
        }
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    SessionManager.AccountService.DeleteAccountEmail(SessionManager.Ticket, id);
                    ReportInfo("Email deleted.");
                    GetData(sender, e);
                }
                break;
            case "Resend":
                {
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    string address = e.Item.Cells[(int)Cells.address].Text;
                    SessionManager.AccountService.ConfirmAccountEmail(SessionManager.Ticket, id);
                    ReportInfo("A confirmation has ben resent to '" + address + "'.");
                }
                break;
            case "SetPrincipal":
                {
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    string address = e.Item.Cells[(int)Cells.address].Text;
                    TransitAccountEmail tae = new TransitAccountEmail();
                    tae.Id = id;
                    tae.Address = address;
                    tae.Principal = true;
                    SessionManager.AccountService.CreateOrUpdateAccountEmail(SessionManager.Ticket, tae);
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
                break;
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountEmail te = new TransitAccountEmail();
        te.Address = inputEmailAddress.Text;
        SessionManager.AccountService.CreateOrUpdateAccountEmail(SessionManager.Ticket, te);
        ReportInfo("A confirmation has ben sent to '" + te.Address + "'.");
        GetData(sender, e);
        inputEmailAddress.Text = string.Empty;
    }
}
