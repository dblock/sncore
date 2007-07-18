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
using SnCore.Data.Hibernate;
using System.Collections.Generic;
using SnCore.Tools.Web;

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

            DomainClass cs = SessionManager.GetDomainClass("AccountEmail");
            inputEmailAddress.MaxLength = cs["Address"].MaxLengthInChars;
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
                    SessionManager.Delete<TransitAccountEmail>(id, SessionManager.AccountService.DeleteAccountEmail);
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
                    SessionManager.CreateOrUpdate<TransitAccountEmail>(
                        tae, SessionManager.AccountService.CreateOrUpdateAccountEmail);
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
                break;
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        IList<TransitAccountEmail> emails = SessionManager.AccountService.GetAccountEmails(
            SessionManager.Ticket, SessionManager.AccountId, null);

        foreach (TransitAccountEmail email in emails)
        {
            if (email.Address.Trim().ToLower() == inputEmailAddress.Text.Trim().ToLower())
            {
                throw new Exception(string.Format("You have already added the e-mail address '{0}'.",
                    Renderer.Render(inputEmailAddress.Text)));
            }
        }

        TransitAccountEmail te = new TransitAccountEmail();
        te.Address = inputEmailAddress.Text;
        SessionManager.CreateOrUpdate<TransitAccountEmail>(
            te, SessionManager.AccountService.CreateOrUpdateAccountEmail);
        ReportInfo(string.Format("A confirmation has ben sent to '{0}'.", 
            Renderer.Render(te.Address)));
        GetData(sender, e);
        inputEmailAddress.Text = string.Empty;
    }
}
