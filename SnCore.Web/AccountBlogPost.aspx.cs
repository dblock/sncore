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
using Wilco.Web.UI.WebControls;
using SnCore.Tools.Drawing;
using System.IO;
using System.Drawing;
using SnCore.Tools;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class AccountBlogPostNew : AuthenticatedPage
{
    public int BlogId
    {
        get
        {
            return GetId("bid");
        }
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? string.Format("AccountBlogView.aspx?id={0}", BlogId) : o.ToString());
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DomainClass cs = SessionManager.GetDomainClass("AccountBlogPost");
            inputTitle.MaxLength = cs["Title"].MaxLengthInChars;

            this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";

            linkBack.NavigateUrl = ReturnUrl;

            TransitAccountBlog blog = SessionManager.BlogService.GetAccountBlogById(SessionManager.Ticket, BlogId);
            labelAccountName.Text = Renderer.Render(blog.AccountName);
            labelBlog.Text = Renderer.Render(blog.Name);
            labelBlogDescription.Text = Renderer.Render(blog.Description);
            linkAccount.HRef = string.Format("AccountView.aspx?id={0}", blog.AccountId);
            imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", blog.AccountPictureId);
            if (!blog.EnableComments) enableComments.Enabled = false;

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountBlogsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(blog.Name, Request, string.Format("AccountBlogEdit.aspx?id={0}", blog.Id)));

            if (RequestId != 0)
            {
                TransitAccountBlogPost post = SessionManager.BlogService.GetAccountBlogPostById(SessionManager.Ticket, RequestId);
                inputBody.Text = post.Body;
                inputTitle.Text = post.Title;
                inputSticky.Checked = post.Sticky;
                inputPublish.Checked = post.Publish;
                labelLastSaved.Text = string.Format("Last saved: {0}", Adjust(post.Modified));
                sitemapdata.Add(new SiteMapDataAttributeNode(post.Title, Request.Url));
                linkPreview.NavigateUrl = string.Format("AccountBlogPostView.aspx?id={0}", RequestId);
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Post", Request.Url));
                linkPreview.Visible = false;
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(linkSave);
    }

    private int saveOnly()
    {
        TransitAccountBlogPost tp = new TransitAccountBlogPost();
        tp.Title = inputTitle.Text;
        tp.Body = inputBody.Text;
        tp.AccountBlogId = BlogId;
        tp.Id = RequestId;
        tp.EnableComments = enableComments.Checked;
        tp.Sticky = inputSticky.Checked;
        tp.Publish = inputPublish.Checked;
        tp.Id = SessionManager.CreateOrUpdate<TransitAccountBlogPost>(
            tp, SessionManager.BlogService.CreateOrUpdateAccountBlogPost);
        SessionManager.InvalidateCache<TransitAccountBlog>();
        return tp.Id;
    }

    public void save(object sender, EventArgs e)
    {
        saveOnly();
        Redirect(ReturnUrl);
    }

    public void saveAndContinue(object sender, EventArgs e)
    {
        int id = saveOnly();
        labelLastSaved.Text = string.Format("Last saved: {0}", Adjust(DateTime.UtcNow));
        if (RequestId == 0)
        {
            Redirect(string.Format("AccountBlogPost.aspx?bid={0}&id={1}", BlogId, id));
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
                    TransitAccountPicture p = new TransitAccountPicture();

                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Bitmap = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    p.Description = string.Empty;
                    p.Hidden = true;

                    int id = SessionManager.CreateOrUpdate<TransitAccountPicture>(
                        p, SessionManager.AccountService.CreateOrUpdateAccountPicture);

                    Size size = t.GetNewSize(new Size(200, 200));

                    inputBody.Text = string.Format("<a href=AccountPictureView.aspx?id={2}><img border=0 width={0} height={1} src=AccountPicture.aspx?id={2}></a>\n{3}",
                        size.Width, size.Height, id, inputBody.Text);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                        Renderer.Render(file.FileName), ex.Message), ex));
                }

                exceptions.Throw();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
