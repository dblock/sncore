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

public partial class AccountAddressesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();

                if (gridManage.Items.Count == 0)
                {
                    ReportInfo("Why not create an address?");
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = AccountService.GetAccountAddresses(SessionManager.Ticket);
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
            switch (e.CommandName)
            {
                case "Delete":
                    AccountService.DeleteAccountAddress(SessionManager.Ticket, id);
                    ReportInfo("Address deleted.");
                    gridManage.CurrentPageIndex = 0;
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
}
