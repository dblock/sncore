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
using Wilco.Web.UI;
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;
using SnCore.Tools.Web;

public partial class AccountStoriesNewViewControl : Control
{
    private int mItemsCollapseAfter = 1;
    private int mItemsShown = 0;
    private int mCount = 5;

    public int ItemsCollapseAfter
    {
        get
        {
            return mItemsCollapseAfter;
        }
        set
        {
            mItemsCollapseAfter = value;
        }
    }

    public int Count
    {
        get
        {
            return mCount;
        }
        set
        {
            mCount = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions(Count, 0);
            storiesView.DataSource = SessionManager.GetCollection<TransitAccountStory>(
                options, SessionManager.StoryService.GetAllAccountStories);
            storiesView.DataBind();
        }
    }

    public string GetDescription(string description)
    {
        if (mItemsShown++ >= mItemsCollapseAfter)
            return string.Empty;

        if (string.IsNullOrEmpty(description))
            return string.Empty;

        return base.GetSummary(description);
    }

    public string GetComments(int count)
    {
        if (count == 0) return "read";
        else if (count == 1) return "read one comment";
        else return string.Format("read {0} comments", count);
    }
}
