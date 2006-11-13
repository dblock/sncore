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

public partial class SearchDiscussionPosts : Page
{
    public void Page_Load()
    {
        try
        {
            SetDefaultButton(search);
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    linkDiscussion.Visible = true;
                    linkDiscussion.NavigateUrl = string.Format("DiscussionView.aspx?id={0}", RequestId);
                    object[] args = { RequestId };
                    TransitDiscussion discussion = SessionManager.GetCachedItem<TransitDiscussion>(
                        DiscussionService, "GetDiscussionById", args);
                    linkDiscussion.Text = Renderer.Render(discussion.Name);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }        
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
        try
        {
            Redirect(string.Format("{0}?id={1}&q={2}",
                Request.Url.AbsolutePath,
                RequestId,
                Renderer.UrlEncode(inputSearch.Text)));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
