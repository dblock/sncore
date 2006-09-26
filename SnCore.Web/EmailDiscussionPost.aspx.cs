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
            try
            {
                if (mDiscussionPost == null)
                {
                    mDiscussionPost = DiscussionService.GetDiscussionPostById(
                        SessionManager.Ticket, RequestId);
                }
            }
            catch (Exception ex)
            {
                ReportException(ex);
            }

            return mDiscussionPost;
        }
    }

    public TransitDiscussionPost DiscussionPostParent
    {
        get
        {
            try
            {
                if (DiscussionPost.DiscussionPostParentId != 0)
                {
                    mDiscussionPostParent = DiscussionService.GetDiscussionPostById(
                        SessionManager.Ticket, DiscussionPost.DiscussionPostParentId);
                }
            }
            catch (Exception ex)
            {
                ReportException(ex);
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
                mDiscussion = DiscussionService.GetDiscussionById(
                    DiscussionPost.DiscussionId);
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
                mRecepient = AccountService.GetAccountById((DiscussionPostParent != null) ? 
                    DiscussionPostParent.AccountId : Discussion.AccountId);
            }

            return mRecepient;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Title = string.Format("New post in {0}", Renderer.Render(Discussion.Name));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string ReturnUrl
    {
        get
        {
            string DiscussionReturnUrl = string.Format("DiscussionView.aspx?id={0}", Discussion.Id);
            string DiscussionPostReturnUrl = string.Format("DiscussionThreadView.aspx?id={0}&did={1}&ReturnUrl={2}",
                DiscussionPost.DiscussionThreadId, Discussion.Id, Renderer.UrlEncode(DiscussionReturnUrl));
            return Renderer.UrlEncode(DiscussionPostReturnUrl);
        }
    }
}
