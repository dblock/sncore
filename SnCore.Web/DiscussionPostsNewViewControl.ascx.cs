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
using System.Collections.Generic;

public partial class DiscussionPostsNewViewControl : Control
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IList<TransitDiscussionPost> items = SessionManager.GetCollection<TransitDiscussionPost>(
                (ServiceQueryOptions) null, SessionManager.DiscussionService.GetLatestDiscussionPosts);
            discussionView.DataSource = items;
            discussionView.DataBind();
        }
    }
}
