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
using SnCore.WebControls;

public partial class DiscussionViewControl : Control
{
    private string mCssClass = "sncore_table";

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

    public int OuterWidth
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "OuterWidth", 680);
        }
        set
        {
            ViewState["OuterWidth"] = value;
        }
    }

    void GetData(object sender, EventArgs e)
    {
        if (DiscussionId <= 0)
            return;

        gridManage.CurrentPageIndex = 0;

        DiscussionViewTypes type = GetDiscussionViewType();
        switch (type)
        {
            case DiscussionViewTypes.FlatWithNewestOnTop:
            case DiscussionViewTypes.FlatFullWithNewestOnTop:
                gridManage.VirtualItemCount = SessionManager.GetCount<TransitDiscussionPost, int>(
                    DiscussionId, SessionManager.DiscussionService.GetDiscussionPostsCount);
                break;
            case DiscussionViewTypes.ThreadedWithNewestOnTop:
            case DiscussionViewTypes.ThreadedFullWithNewestOnTop:
            default:
                gridManage.VirtualItemCount = SessionManager.GetCount<TransitDiscussionPost, int>(
                    DiscussionId, SessionManager.DiscussionService.GetDiscussionThreadsCountByDiscussionId);
                break;
        }

        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    public TransitDiscussion GetDiscussion()
    {
        return SessionManager.GetInstance<TransitDiscussion, int>(
            DiscussionId, SessionManager.DiscussionService.GetDiscussionById);
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        if (DiscussionId <= 0)
            return;

        TransitDiscussion d = GetDiscussion();
        discussionLabel.Text = Renderer.Render(d.Name);
        discussionDescription.Text = Renderer.Render(d.Description);
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;

        DiscussionViewTypes type = GetDiscussionViewType();
        switch (type)
        {
            case DiscussionViewTypes.FlatWithNewestOnTop:
            case DiscussionViewTypes.FlatFullWithNewestOnTop:
                gridManage.DataSource = SessionManager.GetCollection<TransitDiscussionPost, int>(
                    DiscussionId, options, SessionManager.DiscussionService.GetLatestDiscussionPostsById);
                break;
            case DiscussionViewTypes.ThreadedWithNewestOnTop:
            case DiscussionViewTypes.ThreadedFullWithNewestOnTop:
            default:
                gridManage.DataSource = SessionManager.GetCollection<TransitDiscussionPost, int>(
                    DiscussionId, options, SessionManager.DiscussionService.GetDiscussionThreadsByDiscussionId);
                break;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        PageManager.SetDefaultButton(search, panelSearch.Controls);
        if (!IsPostBack && DiscussionId > 0)
        {
            postNew.NavigateUrl = string.Format("DiscussionPost.aspx?did={0}&ReturnUrl={1}",
                DiscussionId, Renderer.UrlEncode(Request.Url.PathAndQuery));

            linkRelRss.NavigateUrl = string.Format("DiscussionRss.aspx?id={0}", DiscussionId);

            gridManage.CssClass = mCssClass;
            tableSearch.Attributes["class"] = mCssClass;

            GetData(sender, e);
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

    public string CssClass
    {
        get
        {
            return mCssClass;
        }
        set
        {
            mCssClass = value;
        }
    }

    protected void search_Click(object sender, EventArgs e)
    {
        Redirect(string.Format("SearchDiscussionPosts.aspx?id={0}&q={1}",
            RequestId,
            Renderer.UrlEncode(inputSearch.Text)));
    }

    public bool IsFull
    {
        get
        {
            DiscussionViewTypes type = GetDiscussionViewType();
            switch (type)
            {
                case DiscussionViewTypes.FlatFullWithNewestOnTop:
                case DiscussionViewTypes.ThreadedFullWithNewestOnTop:
                    return true;
            }

            return false;
        }
    }

    public DiscussionViewTypes GetDiscussionViewType()
    {
        TransitDiscussion d = GetDiscussion();
        DiscussionViewTypes type = DiscussionViewTypes.ThreadedWithNewestOnTop;
        if (!string.IsNullOrEmpty(d.DefaultView))
        {
            type = (DiscussionViewTypes)Enum.Parse(typeof(DiscussionViewTypes), d.DefaultView);
        }
        return type;
    }

    public bool IsThreaded
    {
        get
        {
            DiscussionViewTypes type = GetDiscussionViewType();
            switch (type)
            {
                case DiscussionViewTypes.ThreadedFullWithNewestOnTop:
                case DiscussionViewTypes.ThreadedWithNewestOnTop:
                    return true;
            }

            return false;
        }
    }

}
