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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountStoryPictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);
        if (!IsPostBack)
        {
            mPictureId = RequestId;
            GetPictureData(sender, e);
            GetPicturesData(sender, e);

            TransitAccountStoryPicture p = AccountStoryPicture;
            TransitAccountStory s = AccountStory;
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Stories", Request, "AccountStoriesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(s.Name, Request, string.Format("AccountStoryView.aspx?id={0}", s.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode(string.Format("Pictures > {0}", p.Name), Request.Url));
            StackSiteMap(sitemapdata);
        }

    }

    private int mPictureId = 0;

    public int PictureId
    {
        get
        {
            if (mPictureId == 0)
            {
                mPictureId = RequestId;
            }
            return mPictureId;
        }
    }

    private TransitAccountStoryPicture mAccountStoryPicture = null;

    TransitAccountStoryPicture AccountStoryPicture
    {
        get
        {
            if (mAccountStoryPicture == null)
            {
                mAccountStoryPicture = SessionManager.GetInstance<TransitAccountStoryPicture, int>(
                    PictureId, SessionManager.StoryService.GetAccountStoryPictureById);
            }
            return mAccountStoryPicture;
        }
    }

    private TransitAccountStory mAccountStory = null;

    public TransitAccountStory AccountStory
    {
        get
        {
            if (mAccountStory == null)
            {
                mAccountStory = SessionManager.GetInstance<TransitAccountStory, int>(
                    AccountStoryPicture.AccountStoryId, SessionManager.StoryService.GetAccountStoryById);
            }
            return mAccountStory;
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCount<TransitAccountStoryPicture, int>(
            AccountStory.Id, SessionManager.StoryService.GetAccountStoryPicturesCount);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitAccountStoryPicture p = AccountStoryPicture;

        inputPicture.Src = string.Format("AccountStoryPicture.aspx?id={0}", p.Id);
        inputName.Text = Renderer.Render(p.Name);
        inputCreated.Text = Adjust(p.Created).ToString("d");
        inputCounter.Text = p.Counter.Total.ToString();

        TransitAccountStory l = AccountStory;

        labelAccountStoryName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        linkBack.NavigateUrl = string.Format("AccountStoryView.aspx?id={0}", l.Id);
        linkBack.Text = string.Format("&#187; Back to {0}", Renderer.Render(l.Name));
        linkComments.Visible = p.CommentCount > 0;
        linkComments.Text = string.Format("&#187; {0} comment{1}",
            (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
            (p.CommentCount == 1) ? "" : "s");

        linkPrev.Enabled = p.PrevId > 0;
        linkPrev.CommandArgument = p.PrevId.ToString();
        linkNext.Enabled = p.NextId > 0;
        linkNext.CommandArgument = p.NextId.ToString();
        labelIndex.Text = string.Format("{0} / {1}", p.Index + 1, p.Count);

        discussionComments.DiscussionId = SessionManager.GetCount<TransitDiscussion, string, int>(
            typeof(AccountStoryPicture).Name, PictureId, SessionManager.DiscussionService.GetOrCreateDiscussionId);
        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Picture":
                mPictureId = int.Parse(e.CommandArgument.ToString());
                GetPictureData(source, e);
                break;
        }
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
        picturesView.DataSource = SessionManager.GetCollection<TransitAccountStoryPicture, int>(
            AccountStory.Id, options, SessionManager.StoryService.GetAccountStoryPictures);
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
