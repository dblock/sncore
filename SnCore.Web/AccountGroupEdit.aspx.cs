using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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

public partial class AccountGroupEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsManage.aspx"));

            linkBack.NavigateUrl = ReturnUrl;

            // get group data
            if (RequestId > 0)
            {
                TransitAccountGroup tw = GetAccountGroup();

                inputName.Text = tw.Name;
                inputDescription.Text = tw.Description;
                inputPrivate.Checked = tw.IsPrivate;
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Group", Request.Url));
            }

            GetDataBlogs(sender, e);
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public TransitAccountGroup GetAccountGroup()
    {
        return SessionManager.GetInstance<TransitAccountGroup, int>(
            RequestId, SessionManager.GroupService.GetAccountGroupById);
    }

    private void GetDataBlogs(object sender, EventArgs e)
    {
        // get blogs
        List<TransitAccountBlog> blogs = new List<TransitAccountBlog>();
        blogs.Add(new TransitAccountBlog());
        blogs.AddRange(SessionManager.GetCollection<TransitAccountBlog, int>(
            SessionManager.AccountId, null, SessionManager.BlogService.GetAccountBlogs));
        blogs.AddRange(SessionManager.GetCollection<TransitAccountBlog, int>(
            SessionManager.AccountId, null, SessionManager.BlogService.GetAuthoredAccountBlogs));

        inputBlog.DataSource = blogs;
        inputBlog.DataBind();

        if (RequestId > 0)
        {
            TransitAccountGroup tw = GetAccountGroup();
            if (tw.AccountBlogId > 0)
            {
                inputBlog.ClearSelection();
                ListItem item = inputBlog.Items.FindByValue(tw.AccountBlogId.ToString());
                if (item == null)
                {
                    TransitAccountBlog t_blog = SessionManager.GetInstance<TransitAccountBlog, int>(
                        tw.AccountBlogId, SessionManager.BlogService.GetAccountBlogById);
                    blogs.Add(t_blog);
                    item = inputBlog.Items.FindByValue(tw.AccountBlogId.ToString());
                    inputBlog.DataBind();
                }

                item.Selected = true;
            }
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountGroup tw = new TransitAccountGroup();
        tw.Name = inputName.Text;
        tw.Description = inputDescription.Text;
        tw.IsPrivate = inputPrivate.Checked;
        tw.Id = RequestId;
        tw.AccountBlogId = (! string.IsNullOrEmpty(inputBlog.SelectedValue)) ? int.Parse(inputBlog.SelectedValue) : 0;
        SessionManager.CreateOrUpdate<TransitAccountGroup>(
            tw, SessionManager.GroupService.CreateOrUpdateAccountGroup);
        Redirect(ReturnUrl);
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "AccountGroupsManage.aspx" : o.ToString());
        }
    }
}
