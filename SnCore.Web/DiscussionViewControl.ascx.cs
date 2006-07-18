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
using SnCore.WebServices;

public partial class DiscussionViewControl : Control
{
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

    void GetData()
    {
        discussionView.CurrentPageIndex = 0;
        discussionView.VirtualItemCount = DiscussionService.GetDiscussionThreadsCountById(DiscussionId);
        gridManage_OnGetDataSource(this, null);
        discussionView.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            postNew.NavigateUrl = string.Format("DiscussionPost.aspx?did={0}&ReturnUrl={1}&#edit",
                DiscussionId, Renderer.UrlEncode(Request.Url.PathAndQuery));

            if (DiscussionId > 0)
            {
                TransitDiscussion d = DiscussionService.GetDiscussionById(DiscussionId);
                discussionLabel.Text = Renderer.Render(d.Name);
                discussionDescription.Text = Renderer.Render(d.Description);
                ServiceQueryOptions options = new ServiceQueryOptions();
                options.PageNumber = discussionView.CurrentPageIndex;
                options.PageSize = discussionView.PageSize;
                discussionView.DataSource = DiscussionService.GetDiscussionThreadsById(SessionManager.Ticket, DiscussionId, options);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        discussionView.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        PageManager.SetDefaultButton(search, Controls);
        try
        {
            if (!IsPostBack)
            {
                GetData();
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
            switch (e.Item.ItemType)
            {
                case ListItemType.AlternatingItem:
                case ListItemType.Item:
                case ListItemType.SelectedItem:
                case ListItemType.EditItem:
                    switch (e.CommandName)
                    {
                        case "Edit":
                            {
                                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                                Redirect(
                                    string.Format("DiscussionPost.aspx?did={0}&id={1}&ReturnUrl={2}&#edit",
                                        DiscussionId, id, Renderer.UrlEncode(Request.Url.PathAndQuery)));
                                break;
                            }
                        case "Delete":
                            {
                                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                                DiscussionService.DeleteDiscussionPost(SessionManager.Ticket, id);
                                GetData();
                                break;
                            }
                    }
                    break;
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
        pic,
        content,
        count,
        account,
        posted,
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

                break;
        }
    }

    protected void search_Click(object sender, EventArgs e)
    {
        try
        {
            Redirect(string.Format("SearchDiscussionPosts.aspx?id={0}&q={1}",
                DiscussionId, Renderer.UrlEncode(inputSearch.Text)));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
