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
using Wilco.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Tools.Web;
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountBlogEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManagePosts.OnGetDataSource += new EventHandler(gridManagePosts_OnGetDataSource);
            gridManageAuthors.OnGetDataSource += new EventHandler(gridManageAuthors_OnGetDataSource);

            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountBlogsManage.aspx"));

                if (RequestId > 0)
                {
                    TransitAccountBlog tf = SessionManager.BlogService.GetAccountBlogById(
                        SessionManager.Ticket, RequestId);

                    inputName.Text = tf.Name;
                    inputDescription.Text = tf.Description;

                    if (!IsPostBack)
                    {
                        GetBlogPostsData(sender, e);
                        GetBlogAuthorsData(sender, e);
                    }

                    linkNew.NavigateUrl = string.Format("AccountBlogPost.aspx?bid={0}", RequestId);
                    linkNewAuthor.NavigateUrl = string.Format("AccountBlogAuthorEdit.aspx?bid={0}", RequestId);
                    linkPreview.NavigateUrl = string.Format("AccountBlogView.aspx?id={0}", RequestId);

                    sitemapdata.Add(new SiteMapDataAttributeNode(tf.Name, Request.Url));
                }
                else
                {
                    panelEntries.Visible = false;
                    linkPreview.Visible = false;
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Blog", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(linkSave);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private void GetBlogPostsData(object sender, EventArgs e)
    {
        gridManagePosts.CurrentPageIndex = 0;
        gridManagePosts.VirtualItemCount = SessionManager.BlogService.GetAccountBlogPostsCountById(RequestId);
        gridManagePosts_OnGetDataSource(this, null);
        gridManagePosts.DataBind();
    }

    private void GetBlogAuthorsData(object sender, EventArgs e)
    {
        gridManageAuthors.CurrentPageIndex = 0;
        gridManageAuthors.VirtualItemCount = SessionManager.BlogService.GetAccountBlogAuthorsCountById(RequestId);
        gridManageAuthors_OnGetDataSource(this, null);
        gridManageAuthors.DataBind();
    }

    public void save(object sender, EventArgs e)
    {
        try
        {
            TransitAccountBlog s = new TransitAccountBlog();
            s.Id = RequestId;
            s.Name = inputName.Text;
            s.Description = inputDescription.Text;
            s.AccountId = SessionManager.Account.Id;
            s.Id = SessionManager.BlogService.CreateOrUpdateAccountBlog(SessionManager.Ticket, s);
            // automatically syndicate the blog
            SessionManager.BlogService.SyndicateAccountBlog(SessionManager.Ticket, s.Id);
            Redirect("AccountBlogsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    };

    void gridManagePosts_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManagePosts.CurrentPageIndex;
            options.PageSize = gridManagePosts.PageSize;
            gridManagePosts.DataSource = SessionManager.BlogService.GetAccountBlogPostsById(SessionManager.Ticket, RequestId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManageAuthors_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManageAuthors.CurrentPageIndex;
            options.PageSize = gridManageAuthors.PageSize;
            gridManageAuthors.DataSource = SessionManager.BlogService.GetAccountBlogAuthorsById(RequestId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManagePosts_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    SessionManager.BlogService.DeleteAccountBlogPost(SessionManager.Ticket, id);
                    ReportInfo("Blog post deleted.");
                    gridManagePosts.CurrentPageIndex = 0;
                    gridManagePosts_OnGetDataSource(sender, e);
                    gridManagePosts.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManageAuthors_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
            switch (e.CommandName)
            {
                case "Delete":
                    SessionManager.BlogService.DeleteAccountBlogAuthor(SessionManager.Ticket, id);
                    ReportInfo("Blog author removed.");
                    gridManageAuthors.CurrentPageIndex = 0;
                    gridManageAuthors_OnGetDataSource(sender, e);
                    gridManageAuthors.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
