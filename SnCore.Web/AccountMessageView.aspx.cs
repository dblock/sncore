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
        try
        {
            if (!IsPostBack)
            {
                linkCancel.NavigateUrl = ReturnUrl;

                TransitAccountMessage message = Message;

                if (message.Unread)
                {
                    SessionManager.AccountService.MarkMessageAsReadUnread(SessionManager.Ticket, message.Id, false);
                }

                messageSenderLink.HRef = messageFrom.NavigateUrl = string.Format("AccountView.aspx?id={0}", message.SenderAccountId);
                messageFrom.Text = messageSenderName.Text = Renderer.Render(message.SenderAccountName);
                messageTo.NavigateUrl = string.Format("AccountView.aspx?id={0}", message.RecepientAccountId);
                messageTo.Text = Renderer.Render(message.RecepientAccountName);
                messageSent.Text = message.Sent.ToString();
                messageSenderImage.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}", message.SenderAccountPictureId);
                messageSubject.Text = Renderer.Render(message.Subject);
                messageBody.Text = base.RenderEx(message.Body);

                messageFrom.Visible = labelMessageFrom.Visible = (message.SenderAccountId != SessionManager.Account.Id);
                messageTo.Visible = labelMessageTo.Visible = (message.RecepientAccountId != SessionManager.Account.Id);

                linkReply.NavigateUrl = string.Format("AccountMessageEdit.aspx?id={0}&pid={1}&ReturnUrl={2}#edit",
                    message.SenderAccountId, message.Id, UrlEncode(Request.Url.PathAndQuery));

                linkMove.NavigateUrl = string.Format("AccountMessageMove.aspx?id={0}&ReturnUrl={1}",
                    message.Id, UrlEncode(ReturnUrl));

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Messages", Request, "AccountMessageFoldersManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(message.Subject, Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int trashid = SessionManager.AccountService.GetAccountMessageSystemFolder(SessionManager.Ticket, "trash").Id;
            if (trashid == Message.AccountMessageFolderId)
            {
                SessionManager.AccountService.DeleteAccountMessage(SessionManager.Ticket, RequestId);
            }
            else
            {
                SessionManager.AccountService.MoveAccountMessageToFolderById(SessionManager.Ticket, RequestId, trashid);
            }

            Redirect(ReturnUrl);
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
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "AccountMessageFoldersManage.aspx?folder=inbox" : o.ToString());
        }
    }
}
