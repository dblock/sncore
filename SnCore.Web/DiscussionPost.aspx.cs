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
using Wilco.Web.UI.WebControls;
using SnCore.Tools.Drawing;
using System.IO;
using System.Drawing;
using SnCore.Tools;
using SnCore.SiteMap;

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

    public string ReturnUrl
    {
        get
        {
            string result = Request.Params["ReturnUrl"];
            if (string.IsNullOrEmpty(result) && (DiscussionId > 0)) result = string.Format("DiscussionView.aspx?id={0}", DiscussionId);
            return result;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));

                this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";

                linkCancel.NavigateUrl = ReturnUrl;

                if (DiscussionId > 0)
                {
                    TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(DiscussionId);
                    discussionLabelLink.Text = Renderer.Render(td.Name);
                    discussionLabelLink.NavigateUrl = string.Format("DiscussionView.aspx?id={0}", td.Id);                    
                    sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", td.Id)));
                }

                StringBuilder body = new StringBuilder();

                if (PostId > 0)
                {
                    TransitDiscussionPost tw = SessionManager.DiscussionService.GetDiscussionPostById(SessionManager.Ticket, PostId);
                    titleNewPost.Text = Renderer.Render(tw.Subject);
                    inputSubject.Text = tw.Subject;
                    body.Append(tw.Body);
                    sitemapdata.Add(new SiteMapDataAttributeNode(tw.Subject, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Post", Request.Url));
                }
                
                if (ParentId > 0)
                {
                    TransitDiscussionPost rp = SessionManager.DiscussionService.GetDiscussionPostById(SessionManager.Ticket, ParentId);
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
                        body.AppendFormat("<P>[quote]<DIV>on {0} {1} wrote:</DIV><DIV>{2}</DIV>[/quote]</P>", 
                            rp.Created.ToString("d"), rp.AccountName, rp.Body);
                    }
                }
                
                if ((PostId == 0) && ! string.IsNullOrEmpty(SessionManager.Account.Signature))
                {
                    body.Append("<BR /><BR />");
                    body.Append("<P>");
                    body.Append(Renderer.RenderEx(SessionManager.Account.Signature));
                    body.Append("</P>");
                }

                inputBody.Text = body.ToString();

                StackSiteMap(sitemapdata);
            }

            if (!SessionManager.AccountService.HasVerifiedEmail(SessionManager.Ticket))
            {
                ReportWarning("You don't have any verified e-mail addresses.\n" +
                    "You must add/confirm a valid e-mail address before posting messages.");

                panelPost.Visible = false;
                post.Enabled = false;
            }

            SetDefaultButton(post);
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
            SessionManager.DiscussionService.AddDiscussionPost(SessionManager.Ticket, tw);
            Redirect(linkCancel.NavigateUrl);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected void files_FilesPosted(object sender, FilesPostedEventArgs e)
    {
        try
        {
            if (e.PostedFiles.Count == 0)
                return;

            ExceptionCollection exceptions = new ExceptionCollection();
            foreach (HttpPostedFile file in e.PostedFiles)
            {
                try
                {
                    TransitAccountPictureWithBitmap p = new TransitAccountPictureWithBitmap();

                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Bitmap = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    p.Description = string.Empty;
                    p.Hidden = true;

                    int id = SessionManager.AccountService.AddAccountPicture(SessionManager.Ticket, p);

                    Size size = t.GetNewSize(new Size(200, 200));

                    inputBody.Text = string.Format("<a href=AccountPictureView.aspx?id={2}><img border=0 width={0} height={1} src=AccountPicture.aspx?id={2}></a>\n{3}",
                        size.Width, size.Height, id, inputBody.Text);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                        Renderer.Render(file.FileName), ex.Message), ex));
                }
            }
            exceptions.Throw();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
