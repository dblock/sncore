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

public partial class DiscussionFullViewControl : Control
{
    public new string RenderEx(object s)
    {
        return SessionManager.RenderComments(base.RenderEx(s));
    }

    public string Text
    {
        get
        {
            return discussionLabel.Text;
        }
        set
        {
            discussionLabel.Text = value;
        }
    }

    public string PostNewText
    {
        get
        {
            return postNew.Text;
        }
        set
        {
            postNew.Text = value;
        }
    }


    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                postNew.NavigateUrl = string.Format("DiscussionPost.aspx?did={0}&ReturnUrl={1}&#edit",
                     DiscussionId,
                     Renderer.UrlEncode(Request.Url.PathAndQuery));

                if (DiscussionId > 0)
                {
                    TransitDiscussion d = DiscussionService.GetDiscussionById(DiscussionId);
                    if (string.IsNullOrEmpty(discussionLabel.Text)) discussionLabel.Text = Renderer.Render(d.Name);
                    discussionDescription.Text = Renderer.Render(d.Description);
                    discussionView.DataSource = DiscussionService.GetDiscussionPosts(
                        SessionManager.Ticket, DiscussionId, null);
                    discussionView.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void discussionView_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Edit":
                    {
                        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                        Redirect(
                            string.Format("DiscussionPost.aspx?did={0}&id={1}&ReturnUrl={2}&#edit",
                                DiscussionId,
                                id,
                                Renderer.UrlEncode(Request.Url.PathAndQuery)));
                        break;
                    }
                case "Delete":
                    {
                        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                        DiscussionService.DeleteDiscussionPost(SessionManager.Ticket, id);
                        discussionView.DataSource = DiscussionService.GetDiscussionPosts(
                            SessionManager.Ticket, DiscussionId, null);
                        discussionView.DataBind();
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
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

    public void discussionView_ItemDataBound(object sender, DataGridItemEventArgs e)
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

                LinkButton deleteButton = (LinkButton)e.Item.Cells[(int)Cells.delete].Controls[0];
                deleteButton.Visible = candelete;
                deleteButton.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this post?');");

                LinkButton editButton = (LinkButton)e.Item.Cells[(int)Cells.edit].Controls[0];
                editButton.Visible = canedit;

                break;
        }
    }

}
