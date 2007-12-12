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
using SnCore.SiteMap;
using SnCore.WebServices;

public partial class SearchDiscussionPosts : Page
{
    public void Page_Load()
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();

            if (RequestId > 0)
            {
                TransitDiscussion td = SessionManager.GetInstance<TransitDiscussion, int>(
                    RequestId, SessionManager.DiscussionService.GetDiscussionById);

                if (td.Personal)
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode(td.ParentObjectName,
                        string.Format("{0}&ReturnUrl={1}", td.ParentObjectUri, Renderer.UrlEncode(Request.Url.PathAndQuery))));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
                }

                sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request,
                    string.Format("DiscussionView.aspx?id={0}", td.Id)));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
            }

            sitemapdata.Add(new SiteMapDataAttributeNode("Search", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(search);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(searchDiscussionPosts.SearchQuery))
            {
                searchDiscussionPosts.Visible = false;
            }
            else
            {
                searchDiscussionPosts.DataBind();
            }
        }
    }

    protected void search_Click(object sender, EventArgs e)
    {
        Redirect(string.Format("{0}?id={1}&q={2}",
            Request.Url.AbsolutePath,
            RequestId,
            Renderer.UrlEncode(inputSearch.Text)));
    }
}
