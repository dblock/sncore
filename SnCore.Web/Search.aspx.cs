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
using System.Collections.Generic;
using SnCore.SiteMap;

[SiteMapDataAttribute("Search")]
public partial class Search : AsyncPage
{
    private class SearchViewControl
    {
        public SearchControl SearchControl;
        public View MultiView;
        public LinkButton LinkButton;

        public SearchViewControl(SearchControl s, View m, LinkButton b)
        {
            SearchControl = s;
            MultiView = m;
            LinkButton = b;
        }
    }

    private List<SearchViewControl> mControls = null;

    private List<SearchViewControl> GetSearchControls()
    {
        List<SearchViewControl> result = new List<SearchViewControl>();
        result.Add(new SearchViewControl(searchPlaces, viewPlaces, linkPlaces));
        result.Add(new SearchViewControl(searchAccounts, viewAccounts, linkAccounts));
        result.Add(new SearchViewControl(searchDiscussionPosts, viewDiscussionPosts, linkDiscussionPosts));
        result.Add(new SearchViewControl(searchAccountStories, viewAccountStories, linkAccountStories));
        result.Add(new SearchViewControl(searchAccountFeedItems, viewAccountFeedItems, linkAccountFeedItems));
        result.Add(new SearchViewControl(searchAccountBlogPosts, viewAccountBlogPosts, linkAccountBlogPosts));
        return result;
    }

    public void Page_Load()
    {
        mControls = GetSearchControls();

        if (!IsPostBack)
        {
            inputSearch.Text = SearchQuery;
            searchView.SetActiveView(viewAccounts);
            SetDefaultButton(search);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!string.IsNullOrEmpty(SearchQuery) && !IsPostBack)
        {
            bool found = false;
            foreach (SearchViewControl c in mControls)
            {
                c.LinkButton.Text = string.Format("{0} ({1})", c.LinkButton.Text, c.SearchControl.ResultsCount);
                c.LinkButton.Enabled = (c.SearchControl.ResultsCount > 0);

                if (c.SearchControl.ResultsCount != 0)
                {
                    searchView.SetActiveView(c.MultiView);
                    c.SearchControl.DataBind();
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                panelNoResults.Visible = true;
                panelSearch.Visible = false;
                return;
            }
        }
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

    protected void linkAny_Click(object sender, EventArgs e)
    {
        foreach (SearchViewControl c in mControls)
        {
            if (c.LinkButton == sender)
            {
                searchView.SetActiveView(c.MultiView);
                c.LinkButton.Enabled = (c.SearchControl.ResultsCount > 0);
                if (c.LinkButton.Text.IndexOf("(") < 0)
                {
                    c.LinkButton.Text = string.Format("{0} ({1})", c.LinkButton.Text, c.SearchControl.ResultsCount);
                }
                c.SearchControl.DataBind();
                break;
            }
        }
    }
}
