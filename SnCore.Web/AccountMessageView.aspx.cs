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

public partial class AccountMessageView : AuthenticatedPage
{
    private TransitAccountMessage mMessage = null;

    public TransitAccountMessage Message
    {
        get
        {
            if (mMessage == null)
            {
                mMessage = SessionManager.AccountService.GetAccountMessageById(
                    SessionManager.Ticket, RequestId);
            }
            return mMessage;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkCancel.NavigateUrl = ReturnUrl;

            TransitAccountMessage message = Message;

            if (message.UnRead)
            {
                message.UnRead = false;
                SessionManager.CreateOrUpdate<TransitAccountMessage>(
                    message, SessionManager.AccountService.CreateOrUpdateAccountMessage);
                SessionManager.InvalidateCache<TransitAccountMessageFolder>();
            }

            messageSenderLink.HRef = messageFrom.NavigateUrl = string.Format("AccountView.aspx?id={0}", message.SenderAccountId);
            messageFrom.Text = Renderer.Render(message.SenderAccountName);
            messageTo.NavigateUrl = string.Format("AccountView.aspx?id={0}", message.RecepientAccountId);
            messageTo.Text = Renderer.Render(message.RecepientAccountName);
            messageSent.Text = SessionManager.ToAdjustedString(message.Sent);
            messageSenderImage.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}&width=75&height=75", message.SenderAccountPictureId);
            messageSubject.Text = Renderer.Render(message.Subject);
            messageBody.Text = base.RenderEx(message.Body);

            messageFrom.Visible = labelMessageFrom.Visible = (message.SenderAccountId != SessionManager.Account.Id);
            messageTo.Visible = labelMessageTo.Visible = (message.RecepientAccountId != SessionManager.Account.Id);

            linkReply.NavigateUrl = string.Format("AccountMessageEdit.aspx?id={0}&pid={1}&ReturnUrl={2}",
                message.SenderAccountId, message.Id, UrlEncode(Request.Url.PathAndQuery));

            linkMove.NavigateUrl = string.Format("AccountMessageMove.aspx?id={0}&ReturnUrl={1}",
                message.Id, UrlEncode(ReturnUrl));

            linkFlag.NavigateUrl = string.Format("AccountFlagEdit.aspx?aid={0}&url={1}&type=Spam&ReturnUrl={2}",
                message.SenderAccountId, Renderer.UrlEncode(string.Format("AccountMessageView.aspx?id={0}", message.Id)),
                Renderer.UrlEncode(Request.Url.PathAndQuery));

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Messages", Request, "AccountMessageFoldersManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(message.Subject, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        int trashid = SessionManager.AccountService.GetAccountMessageSystemFolder(
            SessionManager.Ticket, SessionManager.AccountId, "trash").Id;
        if (trashid == Message.AccountMessageFolderId)
        {
            SessionManager.Delete<TransitAccountMessage>(RequestId, SessionManager.AccountService.DeleteAccountMessage);
        }
        else
        {
            SessionManager.AccountService.MoveAccountMessageToFolderById(SessionManager.Ticket, RequestId, trashid);
        }

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
