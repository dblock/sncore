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

public partial class DiscussionPostNew : AuthenticatedPage
{
    public int PostId
    {
        get
        {
            return RequestId;
        }
    }

    public int DiscussionId
    {
        get
        {
            return GetId("did");
        }
    }

    public int ParentId
    {
        get
        {
            return GetId("pid");
        }
    }

    public bool Quote
    {
        get
        {
            object result = Request["Quote"];
            if (result != null) return bool.Parse(result.ToString());
            return false;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(post);
            if (!IsPostBack)
            {
                string ReturnUrl = Request.Params["ReturnUrl"];
                if (ReturnUrl.Length == 0 && DiscussionId > 0) ReturnUrl = "DiscussionView.aspx?id=" + DiscussionId.ToString();
                linkCancel.NavigateUrl = ReturnUrl;
                linkDiscussion.NavigateUrl = ReturnUrl;

                if (DiscussionId > 0)
                {
                    TransitDiscussion td = DiscussionService.GetDiscussionById(DiscussionId);
                    linkDiscussion.Text = Renderer.Render(td.Name);
                    discussionLabel.Text = Renderer.Render(td.Name);
                    discussionDescription.Text = Renderer.Render(td.Description);
                }

                StringBuilder body = new StringBuilder();

                if (PostId > 0)
                {
                    TransitDiscussionPost tw = DiscussionService.GetDiscussionPostById(SessionManager.Ticket, PostId);
                    inputSubject.Text = tw.Subject;
                    body.Append(tw.Body);
                }
                
                if (ParentId > 0)
                {
                    TransitDiscussionPost rp = DiscussionService.GetDiscussionPostById(SessionManager.Ticket, ParentId);
                    panelReplyTo.Visible = true;
                    replytoSenderName.NavigateUrl = accountlink.HRef = "AccountView.aspx?id=" + rp.AccountId.ToString();
                    replytoSenderName.Text = replytoAccount.Text = Renderer.Render(rp.AccountName);
                    replyToBody.Text = base.RenderEx(rp.Body);
                    replytoCreated.Text = rp.Created.ToString();
                    replytoImage.ImageUrl = "AccountPictureThumbnail.aspx?id=" + rp.AccountPictureId.ToString();
                    replytoSubject.Text = Renderer.Render(rp.Subject);
                    inputSubject.Text = rp.Subject.StartsWith("Re:") ? rp.Subject : "Re: " + rp.Subject;

                    if (Quote)
                    {
                        body.Append("[quote]<BR />");
                        body.Append(rp.AccountName);
                        body.Append(" wrote:<BR />");
                        body.Append(rp.Body);
                        body.Append("[/quote]<BR /><BR />");
                    }
                }
                
                if ((ParentId == 0) && (PostId == 0 || ! Quote) && ! string.IsNullOrEmpty(SessionManager.Account.Signature))
                {
                    body.Append("<br>");
                    body.Append(Renderer.RenderEx(SessionManager.Account.Signature));
                }


                inputBody.Text = body.ToString();
            }

            if (!AccountService.HasVerifiedEmail(SessionManager.Ticket))
            {
                ReportWarning("You don't have any verified e-mail addresses.\n" +
                    "You must add/confirm a valid e-mail address before posting messages.");

                post.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void post_Click(object sender, EventArgs e)
    {
        try
        {
            TransitDiscussionPost tw = new TransitDiscussionPost();
            tw.Subject = inputSubject.Text;
            if (string.IsNullOrEmpty(tw.Subject)) tw.Subject = "Untitled";
            tw.Body = inputBody.Text;
            tw.Id = PostId;
            tw.DiscussionPostParentId = ParentId;
            tw.DiscussionId = DiscussionId;
            DiscussionService.AddDiscussionPost(SessionManager.Ticket, tw);
            Redirect(linkCancel.NavigateUrl);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
