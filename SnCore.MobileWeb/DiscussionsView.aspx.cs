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
using SnCore.SiteMap;

public partial class DiscussionsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            if (IsObjectBound)
            {
                int discussion_id = SessionManager.GetCount<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions, string, int>(
                    Type, ObjectId, SessionManager.DiscussionService.GetOrCreateDiscussionId);

                DiscussionService.TransitDiscussion td = SessionManager.GetInstance<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions, int>(
                    discussion_id, SessionManager.DiscussionService.GetDiscussionById);

                sitemapdata.Add(new SiteMapDataAttributeNode(td.ParentObjectName, string.Format("{0}&ReturnUrl={1}", 
                    td.ParentObjectUri, Renderer.UrlEncode(Request.Url.PathAndQuery))));
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions"));
            }
            StackSiteMap(sitemapdata);
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        if (IsObjectBound)
        {
            gridManage.VirtualItemCount = SessionManager.GetCount<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions, string, int>(
                Type, ObjectId, SessionManager.DiscussionService.GetDiscussionsByObjectIdCount);
        }
        else
        {
            gridManage.VirtualItemCount = SessionManager.GetCount<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions>(
                SessionManager.DiscussionService.GetDiscussionsCount);
        }
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        DiscussionService.ServiceQueryOptions options = new DiscussionService.ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        if (IsObjectBound)
        {
            gridManage.DataSource = SessionManager.GetCollection<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions, string, int>(
                Type, ObjectId, options, SessionManager.DiscussionService.GetDiscussionsByObjectId);
        }
        else
        {
            gridManage.DataSource = SessionManager.GetCollection<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions>(
                options, SessionManager.DiscussionService.GetDiscussions);
        }
    }

    public string Type
    {
        get
        {
            return Request["Type"];
        }
    }

    public int ObjectId
    {
        get
        {
            return GetId("ObjectId");
        }
    }

    public bool IsObjectBound
    {
        get
        {
            return (! string.IsNullOrEmpty(Type)) && (ObjectId > 0);
        }
    }
}
