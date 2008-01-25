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
using System.Collections.Generic;
using SnCore.Services;
using SnCore.SiteMap;

[SiteMapDataAttribute("Site Map")]
public partial class SiteMap2 : Page
{
    public class PagedListItem
    {
        private int mPageNumber;
        private int mPageSize;
        private int mStart;
        private int mCount;
        private int mPageCount;

        public int PageNumber
        {
            get
            {
                return mPageNumber;
            }
        }

        public int PageSize
        {
            get
            {
                return mPageSize;
            }
        }

        public int Start
        {
            get
            {
                return mStart;
            }
        }

        public int PageCount
        {
            get
            {
                return mPageCount;
            }
        }

        public int Count
        {
            get
            {
                return mCount;
            }
        }

        public PagedListItem(int pagenumber, int pagesize, int start, int count, int pagecount)
        {
            mPageNumber = pagenumber;
            mPageSize = pagesize;
            mStart = start;
            mCount = count;
            mPageCount = pagecount;
        }
    }

    public List<PagedListItem> GetPagedList(int count)
    {
        const int segment = 100;
        int pagecount = count / segment + 1;
        List<PagedListItem> result = new List<PagedListItem>(pagecount);
        int current = 1;
        int currentpage = 0;
        while (current <= count)
        {
            int delta = count - current + 1;
            result.Add(new PagedListItem(currentpage++, segment, current, (delta > segment) ? segment : delta, pagecount));
            current += segment;
        }
        return result;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkSuggestFeature.HRef =
                string.Format("BugEdit.aspx?pid={0}&type=Suggestion",
                       SessionManager.GetCachedConfiguration(
                        "SnCore.NewFeature.ProjectId", "0"));

            linkReportBug.HRef =
                string.Format("BugEdit.aspx?pid={0}&type=Bug",
                       SessionManager.GetCachedConfiguration(
                        "SnCore.Bug.ProjectId", "0"));

            linkSiteDiscussion.HRef =
                string.Format("DiscussionView.aspx?id={0}",
                       SessionManager.GetCachedConfiguration(
                        "SnCore.Site.DiscussionId", "0"));

            linkBlog.HRef = 
                string.Format("AccountBlogView.aspx?id={0}",
                       SessionManager.GetCachedConfiguration(
                        "SnCore.Blog.Id", "0"));

            AccountActivityQueryOptions aaqo = new AccountActivityQueryOptions();
            listAccounts.DataSource = GetPagedList(SessionManager.SocialService.GetAccountActivityCount(SessionManager.Ticket, aaqo));
            listAccounts.DataBind();

            TransitAccountFeedQueryOptions fqo = new TransitAccountFeedQueryOptions();
            listFeeds.DataSource = GetPagedList(SessionManager.SyndicationService.GetAccountFeedsCount(SessionManager.Ticket, fqo));
            listFeeds.DataBind();

            TransitPlaceQueryOptions pqo = new TransitPlaceQueryOptions();
            listPlaces.DataSource = GetPagedList(SessionManager.PlaceService.GetPlacesCount(SessionManager.Ticket, pqo));
            listPlaces.DataBind();

            TransitAccountEventQueryOptions aeqo = new TransitAccountEventQueryOptions();
            listAccountEvents.DataSource = GetPagedList(SessionManager.EventService.GetAccountEventsCount(SessionManager.Ticket, aeqo));
            listAccountEvents.DataBind();

            listAccountGroups.DataSource = GetPagedList(SessionManager.GroupService.GetAccountGroupsCount(SessionManager.Ticket));
            listAccountGroups.DataBind();

            listStories.DataSource = GetPagedList(SessionManager.StoryService.GetAllAccountStoriesCount(SessionManager.Ticket));
            listStories.DataBind();

            listDiscussions.DataSource = SessionManager.DiscussionService.GetDiscussions(SessionManager.Ticket, null);
            listDiscussions.DataBind();

            listSurveys.DataSource = SessionManager.ObjectService.GetSurveys(SessionManager.Ticket, null);
            listSurveys.DataBind();

            listContentGroups.DataSource = SessionManager.ContentService.GetAllAccountContentGroups(SessionManager.Ticket, null);
            listContentGroups.DataBind();

            listContentGroupNumbers.DataSource = GetPagedList(SessionManager.ContentService.GetAllAccountContentGroupsCount(SessionManager.Ticket));
            listContentGroupNumbers.DataBind();
        }
    }
}
