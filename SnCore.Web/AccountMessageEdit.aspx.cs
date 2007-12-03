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
using System.Text;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class AccountMessageEdit : AuthenticatedPage
{
    public int ParentId
    {
        get
        {
            return GetId("pid");
        }
    }

    public string ReturnUrl
    {
        get
        {
            string result = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(result) && ParentId == 0) result = "AccountMessageFoldersManage.aspx?folder=inbox";
            if (string.IsNullOrEmpty(result)) result = string.Format("AccountMessageView.aspx?id={0}", ParentId);
            return result;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DomainClass cs = SessionManager.GetDomainClass("AccountMessage");
            inputSubject.MaxLength = cs["Subject"].MaxLengthInChars;

            TransitAccount ta = SessionManager.AccountService.GetAccountById(SessionManager.Ticket, RequestId);

            if (ta == null)
            {
                ReportWarning("Account does not exist.");
                panelMessage.Visible = false;
                return;
            }

            imageAccountTo.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}&width=75&height=75", ta.PictureId);
            linkAccountTo.Text = Renderer.Render(ta.Name);
            linkAccountTo.NavigateUrl = linkAccountTo2.HRef = "AccountView.aspx?id=" + ta.Id.ToString();
            linkBack.NavigateUrl = ReturnUrl;

            StringBuilder body = new StringBuilder();

            if (ParentId != 0)
            {
                TransitAccountMessage rp = SessionManager.AccountService.GetAccountMessageById(
                    SessionManager.Ticket, ParentId);
                panelReplyTo.Visible = true;

                messageFrom.NavigateUrl = accountlink.HRef = "AccountView.aspx?id=" + rp.SenderAccountId.ToString();

                messageFrom.Visible = labelMessageFrom.Visible = (rp.SenderAccountId != SessionManager.Account.Id);
                messageTo.Visible = labelMessageTo.Visible = (rp.RecepientAccountId != SessionManager.Account.Id);

                messageFrom.Text = Renderer.Render(rp.SenderAccountName);
                messageBody.Text = RenderEx(rp.Body);
                messageSent.Text = SessionManager.ToAdjustedString(rp.Sent);
                replytoImage.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}&width=75&height=75", rp.SenderAccountPictureId);
                messageSubject.Text = Renderer.Render(rp.Subject);
                inputSubject.Text = rp.Subject.StartsWith("Re:") ? rp.Subject : "Re: " + rp.Subject;

                body.AppendFormat("<P>[quote]<DIV>on {0} {1} wrote:</DIV><DIV>{2}</DIV>[/quote]</P>",
                        rp.Sent.ToString("d"), rp.SenderAccountName, rp.Body);
            }

            if (!string.IsNullOrEmpty(SessionManager.Account.Signature))
            {
                body.Append("<BR /><BR />");
                body.Append("<P>");
                body.Append(Renderer.RenderEx(SessionManager.Account.Signature));
                body.Append("</P>");
            }

            inputBody.Text = body.ToString();

            if (!SessionManager.HasVerifiedEmailAddress())
            {
                ReportWarning("You don't have any verified e-mail addresses.\n" +
                    "You must add/confirm a valid e-mail address before posting messages.");

                manageAdd.Enabled = false;
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(ta.Name, Request, string.Format("AccountView.aspx?id={0}", ta.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Compose Message", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountMessage tw = new TransitAccountMessage();

        tw.Subject = inputSubject.Text;
        if (string.IsNullOrEmpty(tw.Subject)) tw.Subject = "Untitled";
        tw.Body = inputBody.Text;
        tw.RecepientAccountId = tw.AccountId = RequestId;
        tw.AccountMessageFolderId = 0;
        SessionManager.AccountService.CreateOrUpdateAccountMessage(SessionManager.Ticket, tw);
        Redirect(ReturnUrl);
    }
}
