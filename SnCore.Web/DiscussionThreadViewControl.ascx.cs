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

public partial class DiscussionThreadViewControl : Control
{
    public string ReturnUrl
    {
        get
        {
            string result = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(result)) result = "DiscussionThreadsView.aspx";
            return result;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        discussionThreadView.OnGetDataSource += new EventHandler(discussionThreadView_OnGetDataSource);
        if (!IsPostBack)
        {
            linkBack.NavigateUrl = ReturnUrl;

            if (DiscussionId > 0)
            {
                TransitDiscussion d = SessionManager.DiscussionService.GetDiscussionById(
                    SessionManager.Ticket, DiscussionId);
                discussionLabel.Text = Renderer.Render(d.Name);
                discussionDescription.Text = Renderer.Render(d.Description);
                linkNew.NavigateUrl = string.Format("DiscussionPost.aspx?did={0}&ReturnUrl={1}",
                    d.Id, Renderer.UrlEncode(ReturnUrl));
                linkNew.Visible = ! d.Personal;
                linkAllDiscussions.Visible = !d.Personal;
                linkNewPosts.Visible = !d.Personal;
                linkMove.Visible = SessionManager.IsAdministrator && !d.Personal;
                linkMove.NavigateUrl = string.Format("DiscussionThreadMove.aspx?id={0}", DiscussionThreadId);
            }

            GetData(sender, e);
        }
    }

    protected void GetData(object sender, EventArgs e)
    {
        if (DiscussionThreadId <= 0)
            return;

        discussionThreadView.CurrentPageIndex = 0;
        discussionThreadView.VirtualItemCount = SessionManager.DiscussionService.GetDiscussionThreadPostsCount(
            SessionManager.Ticket, DiscussionThreadId);
        discussionThreadView_OnGetDataSource(sender, e);
        discussionThreadView.DataBind();
    }

    void discussionThreadView_OnGetDataSource(object sender, EventArgs e)
    {
        if (DiscussionThreadId <= 0)
            return;

        discussionThreadView.DataSource = SessionManager.DiscussionService.GetDiscussionThreadPosts(
            SessionManager.Ticket, DiscussionThreadId);
    }

    public void discussionThreadView_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.Delete<TransitDiscussionPost>(id, SessionManager.DiscussionService.DeleteDiscussionPost);
                    GetData(sender, e);
                    break;
                }
        }
    }

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

    private enum Cells
    {
        id = 0,
        canedit,
        candelete,
        content,
        reply,
        edit,
        delete
    };

    public void discussionThreadView_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                bool canedit = bool.Parse(e.Item.Cells[(int)Cells.canedit].Text);
                bool candelete = bool.Parse(e.Item.Cells[(int)Cells.candelete].Text);

                HtmlAnchor linkEdit = (HtmlAnchor)e.Item.FindControl("linkEdit");
                linkEdit.Visible = canedit;
                linkEdit.HRef = string.Format("DiscussionPost.aspx?did={0}&id={1}&ReturnUrl={2}",
                    DiscussionId, id, Renderer.UrlEncode(Request.Url.PathAndQuery));

                LinkButton linkDelete = (LinkButton)e.Item.FindControl("linkDelete");
                linkDelete.Visible = candelete;

                break;
        }
    }
}
