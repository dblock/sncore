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

public partial class AccountMessageEdit : AuthenticatedPage
{
    public int ParentId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                TransitAccount ta = AccountService.GetAccountById(RequestId);
                imageAccountTo.ImageUrl = "AccountPictureThumbnail.aspx?id=" + ta.PictureId.ToString();
                linkAccountTo.Text = Renderer.Render(ta.Name);
                linkAccountTo.NavigateUrl = linkAccountTo2.HRef = "AccountView.aspx?id=" + ta.Id.ToString();
                linkBack.NavigateUrl = Renderer.UrlDecode(Request.QueryString["ReturnUrl"]);

                if (ParentId != 0)
                {
                    TransitAccountMessage rp = AccountService.GetAccountMessageById(
                        SessionManager.Ticket, ParentId);
                    panelReplyTo.Visible = true;
                    accountlink.HRef = "AccountView.aspx?id=" + rp.SenderAccountId.ToString();
                    replytoAccount.Text = replytoAccount2.Text = Renderer.Render(rp.SenderAccountName);
                    replyToBody.Text = RenderEx(rp.Body);
                    replytoCreated.Text = rp.Sent.ToString();
                    replytoImage.ImageUrl = "AccountPictureThumbnail.aspx?id=" + rp.SenderAccountPictureId.ToString();
                    replytoSubject.Text = Renderer.Render(rp.Subject);
                    inputSubject.Text = rp.Subject.StartsWith("Re:") ? rp.Subject : "Re: " + rp.Subject;
                    inputBody.Text =
                        "> " + rp.SenderAccountName + " wrote:\n" +
                        "> " + SessionManager.DeleteComments(rp.Body).Replace("\n", "\n> ") + "\n";
                }

                inputBody.Focus();

                if (!AccountService.HasVerifiedEmail(SessionManager.Ticket))
                {
                    ReportWarning("You don't have any verified e-mail addresses.\n" +
                        "You must add/confirm a valid e-mail address before posting messages.");

                    manageAdd.Enabled = false;
                }
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
            TransitAccountMessage tw = new TransitAccountMessage();

            tw.Subject = inputSubject.Text;
            if (string.IsNullOrEmpty(tw.Subject)) tw.Subject = "[no subject]";
            tw.Body = inputBody.Text;
            tw.AccountId = RequestId;
            tw.AccountMessageFolderId = 0;

            AccountService.AddAccountMessage(SessionManager.Ticket, tw);
            Redirect(Request.QueryString["ReturnUrl"]);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
