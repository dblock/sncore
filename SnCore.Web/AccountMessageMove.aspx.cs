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
using SnCore.Services;
using SnCore.WebServices;
using System.Collections.Generic;
using SnCore.SiteMap;

public partial class AccountMessageMove : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkCancel.NavigateUrl = ReturnUrl;

            TransitAccountMessage message = SessionManager.AccountService.GetAccountMessageById(
                SessionManager.Ticket, RequestId);
            messageSenderLink.HRef = messageFrom.NavigateUrl = string.Format("AccountView.aspx?id={0}", message.SenderAccountId);
            messageFrom.Text = messageSenderName.Text = Renderer.Render(message.SenderAccountName);
            messageTo.NavigateUrl = string.Format("AccountView.aspx?id={0}", message.RecepientAccountId);
            messageTo.Text = Renderer.Render(message.RecepientAccountName);
            messageSent.Text = message.Sent.ToString();
            messageSenderImage.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}", message.SenderAccountPictureId);
            messageSubject.Text = Renderer.Render(message.Subject);

            List<TransitAccountMessageFolder> folders = SessionManager.AccountService.GetAccountMessageFolders(
                SessionManager.Ticket, SessionManager.AccountId, null);
            TransitAccountMessageFolder none = new TransitAccountMessageFolder();
            none.FullPath = none.Name = "Please choose ...";
            folders.Insert(0, none);
            listFolders.DataSource = folders;
            listFolders.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Messages", Request, "AccountMessageFoldersManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(message.Subject, Request, string.Format("AccountMessageView.aspx?id={0}", message.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Move", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void listFolders_SelectedIndexChanged(object sender, EventArgs e)
    {
        int dest_id = int.Parse(listFolders.SelectedValue);
        SessionManager.AccountService.MoveAccountMessageToFolderById(SessionManager.Ticket, RequestId, dest_id);
        Redirect(ReturnUrl);
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "AccountMessageFoldersManage.aspx?folder=inbox" : o.ToString());
        }
    }
}
