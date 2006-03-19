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

public partial class Search : Page
{
    public void Page_Load()
    {
        if (!IsPostBack)
        {
            inputSearch.Text = SearchQuery;
            searchView.SetActiveView(viewAccounts);
            SetDefaultButton(search);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (!string.IsNullOrEmpty(SearchQuery) && !IsPostBack)
        {
            if (searchAccounts.ResultsCount != 0) searchView.SetActiveView(viewAccounts);
            else if (searchPlaces.ResultsCount != 0) searchView.SetActiveView(viewPlaces);
            else if (searchDiscussionPosts.ResultsCount != 0) searchView.SetActiveView(viewDiscussionPosts);
            else if (searchAccountStories.ResultsCount != 0) searchView.SetActiveView(viewAccountStories);
            else if (searchAccountFeedItems.ResultsCount != 0) searchView.SetActiveView(viewAccountFeedItems);
            else if (searchAccountBlogPosts.ResultsCount != 0) searchView.SetActiveView(viewAccountBlogPosts);
        }

        if (!IsPostBack)
        {
            linkAccountFeedItems.Text = string.Format("{0} ({1})", linkAccountFeedItems.Text, searchAccountFeedItems.ResultsCount);
            linkAccountStories.Text = string.Format("{0} ({1})", linkAccountStories.Text, searchAccountStories.ResultsCount);
            linkAccounts.Text = string.Format("{0} ({1})", linkAccounts.Text, searchAccounts.ResultsCount);
            linkDiscussionPosts.Text = string.Format("{0} ({1})", linkDiscussionPosts.Text, searchDiscussionPosts.ResultsCount);
            linkPlaces.Text = string.Format("{0} ({1})", linkPlaces.Text, searchPlaces.ResultsCount);
            linkAccountBlogPosts.Text = string.Format("{0} ({1})", linkAccountBlogPosts.Text, searchAccountBlogPosts.ResultsCount);
        }

        linkAccounts.Enabled = !string.IsNullOrEmpty(SearchQuery) && !viewAccounts.Visible;
        linkAccountStories.Enabled = !string.IsNullOrEmpty(SearchQuery) && !viewAccountStories.Visible;
        linkAccountFeedItems.Enabled = !string.IsNullOrEmpty(SearchQuery) && !viewAccountFeedItems.Visible;
        linkDiscussionPosts.Enabled = !string.IsNullOrEmpty(SearchQuery) && !viewDiscussionPosts.Visible;
        linkPlaces.Enabled = !string.IsNullOrEmpty(SearchQuery) && !viewPlaces.Visible;
        linkAccountBlogPosts.Enabled = !string.IsNullOrEmpty(SearchQuery) && !viewAccountBlogPosts.Visible;

        base.OnPreRender(e);
    }

    public string SearchQuery
    {
        get
        {
            object query = Request.QueryString["q"];
            return query == null ? string.Empty : query.ToString();
        }
    }

    protected void search_Click(object sender, EventArgs e)
    {
        try
        {
            Redirect("Search.aspx?q=" + Renderer.UrlEncode(inputSearch.Text));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected void linkAccounts_Click(object sender, EventArgs e)
    {
        searchView.SetActiveView(viewAccounts);
    }

    protected void linkDiscussionPosts_Click(object sender, EventArgs e)
    {
        searchView.SetActiveView(viewDiscussionPosts);
    }

    protected void linkAccountStories_Click(object sender, EventArgs e)
    {
        searchView.SetActiveView(viewAccountStories);
    }

    protected void linkAccountFeedItems_Click(object sender, EventArgs e)
    {
        searchView.SetActiveView(viewAccountFeedItems);
    }

    protected void linkAccountBlogPosts_Click(object sender, EventArgs e)
    {
        searchView.SetActiveView(viewAccountBlogPosts);
    }

    protected void linkPlaces_Click(object sender, EventArgs e)
    {
        searchView.SetActiveView(viewPlaces);
    }
}
