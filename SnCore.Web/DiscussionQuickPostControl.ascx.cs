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
using Wilco.Web.UI;
using Wilco.Web.UI.WebControls;
using SnCore.Services;
using System.Collections.Generic;

public partial class DiscussionQuickPostControl : Control
{
    public int DiscussionId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "DiscussionId", 0);
        }
        set
        {
            ViewState["DiscussionId"] = value;
        }
    }

    public int DiscussionThreadId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "DiscussionThreadId", 0);
        }
        set
        {
            ViewState["DiscussionThreadId"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionManager.IsLoggedIn)
        {
            panelQuickPost.Enabled = false;
            inputPost.Text = "Please login to post.";
        }
    }

    public void post_Click(object sender, EventArgs e)
    {
        TransitDiscussionPost tw = new TransitDiscussionPost();
        tw.Subject = "Untitled";
        tw.Body = inputPost.Text;
        tw.DiscussionId = DiscussionId;
        tw.DiscussionThreadId = DiscussionThreadId;
        SessionManager.CreateOrUpdate<TransitDiscussionPost>(
            tw, SessionManager.DiscussionService.CreateOrUpdateDiscussionPost);
        SessionManager.InvalidateCache<TransitDiscussion>();
        SessionManager.InvalidateCache<TransitDiscussionThread>();
        Redirect(Request.Url.PathAndQuery);
    }
}
