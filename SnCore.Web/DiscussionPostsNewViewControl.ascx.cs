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
        try
        {
            if (!IsPostBack)
            {
                List<TransitDiscussionPost> items = (List<TransitDiscussionPost>)
                    Cache[string.Format("activeDiscussionPosts:{0}", ClientID)];

                if (items == null)
                {
                    items = DiscussionService.GetLatestDiscussionPosts();
                    Cache.Insert(string.Format("activeDiscussionPosts:{0}", ClientID),
                        items, null, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                }

                discussionView.DataSource = items;
                discussionView.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
