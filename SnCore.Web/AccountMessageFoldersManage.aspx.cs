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
using Wilco.Web.UI;
using Wilco.Web.UI.WebControls;
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountMessageFoldersManage : AuthenticatedPage
{
    public void linkFolder_Click(object sender, CommandEventArgs e)
    {
        try
        {
            FolderId = int.Parse(e.CommandArgument.ToString());
            GetData();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public int FolderId
    {
        get
        {
            int result = ViewStateUtility.GetViewStateValue<int>(ViewState, "FolderId", RequestId);
            
            if (result == 0)
            {
                ViewState["FolderId"] = result = AccountService.GetAccountMessageSystemFolder(
                    SessionManager.Ticket, FolderName).Id;
            }

            return result;
        }
        set
        {
            ViewState["FolderId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
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

    protected override void OnInit(EventArgs e)
    {
        linkEmpty.Attributes.Add("onclick", "return confirm('Are you sure you want to delete all messages in this folder?');");
        base.OnInit(e);
    }

    public string FolderName
    {
        get
        {
            object folder = Request.Params["folder"];
            return (folder != null) ? folder.ToString() : "inbox";
        }
    }

    public void GetData()
    {
        AccountService.CreateAccountSystemMessageFolders(SessionManager.Ticket);

        messagefoldersView.DataSource = AccountService.GetAccountMessageFolders(SessionManager.Ticket);
        messagefoldersView.DataBind();

        messagesView.DataSource = AccountService.GetAccountMessages(SessionManager.Ticket, FolderId);
        messagesView.DataBind();

        TransitAccountMessageFolder folder = AccountService.GetAccountMessageFolderById(SessionManager.Ticket, FolderId);
        labelFolderName.Text = "Folder \"" + Renderer.Render(folder.Name) + "\".";

        emptyPanel.Visible = ((folder.Name == "trash") || (folder.Name == "sent")) && folder.System;

        if (messagesView.Items.Count == 0)
        {
            messagesPanel.Visible = false;
            noticeFolder.Info = string.Format("There're no messages in the '{0}' folder.", folder.Name);
        }
        else
        {
            messagesPanel.Visible = true;
            noticeFolder.Info = string.Empty;
        }
    }

    private enum Cells
    {
        id = 0,
        system,
        content,
        newfolder,
        edit,
        delete
    };

    public void messagefoldersView_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                bool system = bool.Parse(e.Item.Cells[(int)Cells.system].Text);

                LinkButton deleteButton = (LinkButton)e.Item.Cells[(int)Cells.delete].Controls[0];
                deleteButton.Visible = !system;
                deleteButton.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this folder, all its subfolders and messages?');");
                LinkButton editButton = (LinkButton)e.Item.Cells[(int)Cells.edit].Controls[0];
                editButton.Visible = !system;

                break;
        }
    }

    public void messagefoldersView_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Edit":
                    {
                        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                        Redirect("AccountMessageFolderEdit.aspx?id=" + id.ToString());
                        break;
                    }
                case "Delete":
                    {
                        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                        AccountService.DeleteAccountMessageFolder(SessionManager.Ticket, id);

                        if (id == FolderId)
                        {
                            Redirect("AccountMessageFoldersManage.aspx?folder=inbox");
                            break;
                        }

                        messagefoldersView.DataSource = AccountService.GetAccountMessageFolders(SessionManager.Ticket);
                        messagefoldersView.DataBind();
                        break;
                    }
                case "New":
                    {
                        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                        Redirect("AccountMessageFolderEdit.aspx?pid=" + id.ToString());
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string ReturnUrl
    {
        get
        {
            return "AccountMessageFoldersManage.aspx?id=" + FolderId.ToString();
        }
    }

    private enum messageCells
    {
        id = 0,
        content,
        reply,
        delete
    };

    public void messagesView_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    {
                        int id = int.Parse(e.Item.Cells[(int) messageCells.id].Text);
                        int trashid = AccountService.GetAccountMessageSystemFolder(SessionManager.Ticket, "trash").Id;
                        if (trashid == FolderId)
                        {
                            AccountService.DeleteAccountMessage(SessionManager.Ticket, id);
                        }
                        else
                        {
                            AccountService.MoveAccountMessageToFolderById(SessionManager.Ticket, id, trashid);
                        }
                        messagesView.DataSource = AccountService.GetAccountMessages(SessionManager.Ticket, FolderId);
                        messagesView.DataBind();
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkEmpty_Click(object s, EventArgs e)
    {
        try
        {
            AccountService.DeleteAccountMessagesByFolder(SessionManager.Ticket, FolderId);
            messagesView.DataSource = AccountService.GetAccountMessages(SessionManager.Ticket, FolderId);
            messagesView.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetFolderPicture(string name, bool system)
    {
        if (system)
        {
            switch (name)
            {
                case "trash":
                case "inbox":
                case "sent":
                    return string.Format("images/account/{0}.gif", name);
            }
        }

        return "images/Folder.gif";
    }
}

