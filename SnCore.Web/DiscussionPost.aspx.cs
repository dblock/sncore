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

                if (PostId > 0)
                {
                    TransitDiscussionPost tw = DiscussionService.GetDiscussionPostById(SessionManager.Ticket, PostId);
                    inputSubject.Text = tw.Subject;
                    inputBody.Text = tw.Body;
                }

                if (ParentId > 0)
                {
                    TransitDiscussionPost rp = DiscussionService.GetDiscussionPostById(SessionManager.Ticket, ParentId);
                    panelReplyTo.Visible = true;
                    accountlink.HRef = "AccountView.aspx?id=" + rp.AccountId.ToString();
                    replytoAccount.Text = Renderer.Render(rp.AccountName);
                    replyToBody.Text = base.RenderEx(SessionManager.RenderComments(rp.Body));
                    replytoCreated.Text = rp.Created.ToString();
                    replytoImage.ImageUrl = "AccountPictureThumbnail.aspx?id=" + rp.AccountPictureId.ToString();
                    replytoSubject.Text = Renderer.Render(rp.Subject);
                    inputSubject.Text = rp.Subject.StartsWith("Re:") ? rp.Subject : "Re: " + rp.Subject;

                    if (Quote)
                    {
                        inputBody.Text =
                            "> " + rp.AccountName + " wrote:\n" +
                            "> " + SessionManager.DeleteComments(rp.Body).Replace("\n", "\n> ") + "\n\n";
                    }
                }

                if (PostId == 0 && !string.IsNullOrEmpty(SessionManager.Account.Signature))
                {
                    inputSignature.Text = SessionManager.Account.Signature;
                }
                else
                {
                    inputSignature.Enabled = false;
                }

                inputBody.Focus();
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
            if (string.IsNullOrEmpty(tw.Subject)) tw.Subject = "[no subject]";
            tw.Body = inputBody.Text;
            if (!string.IsNullOrEmpty(inputSignature.Text))
            {
                tw.Body = tw.Body + "\n" + inputSignature.Text;
            }
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
