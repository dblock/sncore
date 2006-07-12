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

public partial class AccountEventsManage : AuthenticatedPage
{

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    };

    public void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = EventService.GetAccountEventsCount(SessionManager.Ticket);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageSize = gridManage.PageSize;
            options.PageNumber = gridManage.CurrentPageIndex;
            gridManage.DataSource = EventService.GetAccountEvents(SessionManager.Ticket, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    EventService.DeleteAccountEvent(SessionManager.Ticket, id);
                    ReportInfo("Event deleted.");
                    GetData();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
