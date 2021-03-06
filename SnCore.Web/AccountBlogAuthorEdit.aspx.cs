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
using SnCore.SiteMap;

public partial class AccountBlogAuthorEdit : AuthenticatedPage
{
    public int BlogId
    {
        get
        {
            return GetId("bid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountBlogsManage.aspx"));

            TransitAccountBlog tb = SessionManager.BlogService.GetAccountBlogById(SessionManager.Ticket, BlogId);
            sitemapdata.Add(new SiteMapDataAttributeNode(tb.Name, Request, string.Format("AccountBlogEdit.aspx?id={0}", tb.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Authors", Request, string.Format("AccountBlogEdit.aspx?id={0}#authors", tb.Id)));

            linkBack.NavigateUrl = string.Format("AccountBlogEdit.aspx?id={0}", BlogId);

            if (RequestId != 0)
            {
                TransitAccountBlogAuthor author = SessionManager.BlogService.GetAccountBlogAuthorById(SessionManager.Ticket, RequestId);
                inputId.Text = string.Format("{0} ({1})", author.AccountName, author.AccountId);
                inputId.Enabled = false;
                allowDelete.Checked = author.AllowDelete;
                allowEdit.Checked = author.AllowEdit;
                allowPost.Checked = author.AllowPost;
                sitemapdata.Add(new SiteMapDataAttributeNode(author.AccountName, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Author", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountBlogAuthor ta = new TransitAccountBlogAuthor();
        ta.AccountId = (RequestId > 0) ? RequestId : int.Parse(inputId.Text);
        ta.AccountBlogId = BlogId;
        ta.Id = RequestId;
        ta.AllowDelete = allowDelete.Checked;
        ta.AllowEdit = allowEdit.Checked;
        ta.AllowPost = allowPost.Checked;
        SessionManager.CreateOrUpdate<TransitAccountBlogAuthor>(
            ta, SessionManager.BlogService.CreateOrUpdateAccountBlogAuthor);
        Redirect(string.Format("AccountBlogEdit.aspx?id={0}", BlogId));
    }
}
