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
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;
using System.Text;
using System.Collections.Generic;
using SnCore.Tools.Web;

public partial class DiscussionPostsNewViewControl : Control
{
    private int mHideIfOlder = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = discussionView.PageSize;
            IList<TransitDiscussionPost> items = SessionManager.GetCollection<TransitDiscussionPost>(
                options, SessionManager.DiscussionService.GetDiscussionTopOfThreads);

            if (items.Count > 0 && HideIfOlder > 0 && items[0].DiscussionThreadModified.AddDays(HideIfOlder) < DateTime.UtcNow)
                this.Visible = false;

            discussionView.DataSource = items;
            discussionView.DataBind();
        }
    }

    public int HideIfOlder
    {
        get
        {
            return mHideIfOlder;
        }
        set
        {
            mHideIfOlder = value;
        }
    }

    public string GetReplies(int post_id, int thread_id)
    {
        StringBuilder result = new StringBuilder();
        
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = 0;
        options.PageSize = 3;
        IList<TransitDiscussionPost> items = SessionManager.GetCollection<TransitDiscussionPost, int>(
            thread_id, options, SessionManager.DiscussionService.GetDiscussionThreadPostsByOrder);
        
        foreach (TransitDiscussionPost t_instance in items)
        {
            if (t_instance.Id == post_id)
                continue;

            result.Append(string.Format("<div>&#187; <b>{0}</b> replied {1}</a></div>",
                Renderer.Render(t_instance.AccountName), 
                SessionManager.ToAdjustedString(t_instance.Created)));
        }
        
        if (result.Length > 0)
        {
            result.Insert(0, "<div class=\"sncore_message_reply\">");
            result.Append("</div>");
        }

        return result.ToString();
    }
}
