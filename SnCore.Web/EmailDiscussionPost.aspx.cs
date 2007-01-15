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
using SnCore.Services;
using SnCore.Tools.Web;

public partial class EmailDiscussionPost : AuthenticatedPage
{
    private TransitDiscussionPost mDiscussionPost;
    private TransitDiscussionPost mDiscussionPostParent;

    public TransitDiscussionPost DiscussionPost
    {
        get
        {
            if (mDiscussionPost == null)
            {
                mDiscussionPost = SessionManager.DiscussionService.GetDiscussionPostById(
                    SessionManager.Ticket, RequestId);
            }
            return mDiscussionPost;
        }
    }

    public TransitDiscussionPost DiscussionPostParent
    {
        get
        {
            if (DiscussionPost.DiscussionPostParentId != 0)
            {
                mDiscussionPostParent = SessionManager.DiscussionService.GetDiscussionPostById(
                    SessionManager.Ticket, DiscussionPost.DiscussionPostParentId);
            }
            return mDiscussionPostParent;
        }
    }

    private TransitDiscussion mDiscussion = null;

    public TransitDiscussion Discussion
    {
        get
        {
            if (mDiscussion == null)
            {
                mDiscussion = SessionManager.DiscussionService.GetDiscussionById(
                    SessionManager.Ticket, DiscussionPost.DiscussionId);
            }

            return mDiscussion;
        }
    }

    private TransitAccount mRecepient = null;

    public TransitAccount Recepient
    {
        get
        {
            if (mRecepient == null)
            {
                mRecepient = SessionManager.AccountService.GetAccountById(SessionManager.Ticket, 
                    (DiscussionPostParent != null) ? DiscussionPostParent.AccountId : Discussion.AccountId);
            }

            return mRecepient;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("New post in {0}", Renderer.Render(Discussion.Name));
    }
}

