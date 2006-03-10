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

public partial class AccountEmailsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = AccountService.GetAccountEmails(SessionManager.Ticket);
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
        try
        {
            int id = int.Parse(e.Item.Cells[(int) Cells.id].Text);
            string address = e.Item.Cells[(int) Cells.address].Text;
            switch (e.CommandName)
            {
                case "Delete":
                    AccountService.DeleteAccountEmail(SessionManager.Ticket, id);
                    ReportInfo("Email deleted.");
                    gridManage.CurrentPageIndex = 0;
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
                case "Resend":
                    AccountService.ConfirmAccountEmail(SessionManager.Ticket, id);
                    ReportInfo("A confirmation has ben resent to '" + address + "'.");
                    break;
                case "SetPrincipal":
                    TransitAccountEmail tae = new TransitAccountEmail();
                    tae.Id = id;
                    tae.Address = address;
                    tae.Principal = true;
                    AccountService.UpdateAccountEmail(SessionManager.Ticket, tae);
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitAccountEmail te = new TransitAccountEmail();
            te.Address = inputEmailAddress.Text;
            AccountService.AddAccountEmail(SessionManager.Ticket, te);
            ReportInfo("A confirmation has ben sent to '" + te.Address + "'.");
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
            inputEmailAddress.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
