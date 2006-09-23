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

public partial class AccountStoryPictureView : Page
{
    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                GetPictureData(RequestId);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }

    void GetPictureData(int id)
    {
        object[] sp_args = { SessionManager.Ticket, id };
        TransitAccountStoryPicture p = SessionManager.GetCachedItem<TransitAccountStoryPicture>(
            StoryService, "GetAccountStoryPictureById", sp_args);

        inputPicture.Src = string.Format("AccountStoryPicture.aspx?id={0}", id);
        inputName.Text = Renderer.Render(p.Name);
        inputCreated.Text = Adjust(p.Created).ToString();

        object[] as_args = { SessionManager.Ticket, p.AccountStoryId };
        TransitAccountStory l = SessionManager.GetCachedItem<TransitAccountStory>(
            StoryService, "GetAccountStoryById", as_args);

        labelAccountStoryName.Text = this.Title = string.Format("{0}: {1}", 
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        linkAccountStory.Text = Renderer.Render(l.Name);
        linkAccountStory.NavigateUrl = "AccountStoryView.aspx?id=" + p.AccountStoryId;
        labelPicture.Text = Renderer.Render((p.Name != null && p.Name.Length > 0) ? p.Name : "Untitled");

        linkBack.NavigateUrl = linkAccountStory.NavigateUrl = string.Format("AccountStoryView.aspx?id={0}", l.Id);
        linkBack.Text = string.Format("&#187; Back to {0}", Renderer.Render(l.Name));
        linkComments.Visible = p.CommentCount > 0;
        linkComments.Text = string.Format("&#187; {0} comment{1}",
            (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
            (p.CommentCount == 1) ? "" : "s");

        if (!IsPostBack)
        {
            object[] p_args = { l.Id, null };
            picturesView.DataSource = SessionManager.GetCachedCollection<TransitAccountStoryPicture>(
                StoryService, "GetAccountStoryPicturesById", p_args);
            picturesView.DataBind();
        }

        discussionComments.DiscussionId = DiscussionService.GetAccountStoryPictureDiscussionId(id);
        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Picture":
                    GetPictureData(int.Parse(e.CommandArgument.ToString()));
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
