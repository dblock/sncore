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

public partial class AccountFriendRequestsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridSent.OnGetDataSource += new EventHandler(gridSent_OnGetDataSource);
            gridPending.OnGetDataSource += new EventHandler(gridPending_OnGetDataSource);
            
            if (!IsPostBack)
            {
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridPending_OnGetDataSource(object sender, EventArgs e)
    {
        gridPending.DataSource = SocialService.GetAccountFriendRequests(SessionManager.Ticket);
    }

    void gridSent_OnGetDataSource(object sender, EventArgs e)
    {
        gridSent.DataSource = SocialService.GetAccountFriendRequestsSent(SessionManager.Ticket);
    }

    public void GetData(object sender, EventArgs e)
    {
        gridPending_OnGetDataSource(sender, e);
        gridSent_OnGetDataSource(sender, e);

        gridSent.DataBind();       
        gridPending.DataBind();

        panelSent.Visible = gridSent.Items.Count > 0;
        panelPending.Visible = gridPending.Items.Count > 0;

        if (gridSent.Items.Count + gridPending.Items.Count == 0)
        {
            noticeManage.Info = "You don't have any pending or outstanding requests.";
        }
    }

    public void gridPending_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Accept":
                    {
                        int id = int.Parse(e.Item.Cells[(int)gridSentCells.id].Text);
                        SocialService.AcceptAccountFriendRequest(SessionManager.Ticket, id, inputReason.Text);
                        GetData(sender, e);
                        noticeManage.Info = "Friend request accepted.";
                        break;
                    }
                case "Reject":
                    {
                        int id = int.Parse(e.Item.Cells[(int)gridSentCells.id].Text);
                        SocialService.RejectAccountFriendRequest(SessionManager.Ticket, id, inputReason.Text);
                        GetData(sender, e);
                        noticeManage.Info = "Friend request rejected.";
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public enum gridSentCells
    {
        id = 0,
        data,
        created,
        cancel
    };

    public void gridSent_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Cancel":
                    int id = int.Parse(e.Item.Cells[(int)gridSentCells.id].Text);
                    SocialService.DeleteAccountFriendRequest(SessionManager.Ticket, id);
                    GetData(sender, e);
                    noticeManage.Info = "Request cancelled.";
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
