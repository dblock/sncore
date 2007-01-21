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
using SnCore.SiteMap;
using System.Text;

public partial class AccountMessageFoldersManage : AuthenticatedPage
{
    public void linkFolder_Click(object sender, CommandEventArgs e)
    {
        FolderId = int.Parse(e.CommandArgument.ToString());
        GetMessagesData(sender, e);
    }

    public int FolderId
    {
        get
        {
            int result = ViewStateUtility.GetViewStateValue<int>(ViewState, "FolderId", RequestId);

            if (result == 0)
            {
                ViewState["FolderId"] = result = SessionManager.AccountService.GetAccountMessageSystemFolder(
                    SessionManager.Ticket, SessionManager.AccountId, FolderName).Id;
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
        messagefoldersView.OnGetDataSource += new EventHandler(messagefoldersView_OnGetDataSource);
        messagesView.OnGetDataSource += new EventHandler(messagesView_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Messages", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetMessagesData(object sender, EventArgs e)
    {
        messagesView.CurrentPageIndex = 0;
        messagesView.VirtualItemCount = SessionManager.AccountService.GetAccountMessagesCount(
            SessionManager.Ticket, FolderId);
        messagesView_OnGetDataSource(sender, e);
        messagesView.DataBind();

        GetFolderInformationData(sender, e);
    }

    void GetFolderInformationData(object sender, EventArgs e)
    {
        TransitAccountMessageFolder folder = SessionManager.AccountService.GetAccountMessageFolderById(
            SessionManager.Ticket, FolderId);

        labelFolderName.Text = Renderer.Render(folder.Name);
        emptyPanel.Visible = ((folder.Name == "trash") || (folder.Name == "sent")) && folder.System;

        if (messagesView.Items.Count == 0)
        {
            noticeFolder.Text = "no messages";
        }
        else
        {
            StringBuilder noticefoldertext = new StringBuilder();
            noticefoldertext.AppendFormat("{0} message{1}", folder.MessageCount, folder.MessageCount == 1 ? string.Empty : "s");
            if (folder.UnReadMessageCount > 0) noticefoldertext.AppendFormat(", {0} unread", folder.UnReadMessageCount);
            noticeFolder.Text = noticefoldertext.ToString();
        }
    }

    void messagesView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = messagesView.CurrentPageIndex;
        options.PageSize = messagesView.PageSize;
        messagesView.DataSource = SessionManager.AccountService.GetAccountMessages(
            SessionManager.Ticket, FolderId, options);
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

    void GetFoldersData(object sender, EventArgs e)
    {
        messagefoldersView.CurrentPageIndex = 0;
        messagefoldersView.VirtualItemCount = SessionManager.AccountService.GetAccountMessageFoldersCount(
            SessionManager.Ticket, SessionManager.AccountId);
        messagefoldersView_OnGetDataSource(sender, e);
        messagefoldersView.DataBind();
    }

    void messagefoldersView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = messagefoldersView.CurrentPageIndex;
        options.PageSize = messagefoldersView.PageSize;
        messagefoldersView.DataSource = SessionManager.AccountService.GetAccountMessageFolders(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    public void GetData(object sender, EventArgs e)
    {
        SessionManager.AccountService.CreateAccountSystemMessageFolders(
            SessionManager.Ticket, SessionManager.AccountId);

        GetFoldersData(sender, e);
        GetMessagesData(sender, e);
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
                    SessionManager.AccountService.DeleteAccountMessageFolder(SessionManager.Ticket, id);

                    if (id == FolderId)
                    {
                        Redirect("AccountMessageFoldersManage.aspx?folder=inbox");
                        break;
                    }

                    messagefoldersView.DataSource = SessionManager.AccountService.GetAccountMessageFolders(
                        SessionManager.Ticket, SessionManager.AccountId, null);
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
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.Item.Cells[(int)messageCells.id].Text);
                    int trashid = SessionManager.AccountService.GetAccountMessageSystemFolder(
                        SessionManager.Ticket, SessionManager.AccountId, "trash").Id;
                    if (trashid == FolderId)
                    {
                        SessionManager.AccountService.DeleteAccountMessage(SessionManager.Ticket, id);
                    }
                    else
                    {
                        SessionManager.AccountService.MoveAccountMessageToFolderById(SessionManager.Ticket, id, trashid);
                    }
                    messagesView.DataSource = SessionManager.AccountService.GetAccountMessages(
                        SessionManager.Ticket, FolderId, null);
                    messagesView.DataBind();
                    break;
                }
        }
    }

    public void linkEmpty_Click(object sender, EventArgs e)
    {
        SessionManager.AccountService.DeleteAccountMessagesByFolder(SessionManager.Ticket, FolderId);
        GetMessagesData(sender, e);
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

    public string GetMessageCssClass(bool unread)
    {
        return (unread ? "sncore_message_unread" : "sncore_message_read");
    }

    public string GetPictureDisplayStyle(bool unread)
    {
        return (unread ? string.Empty : "display: none;");
    }
}

