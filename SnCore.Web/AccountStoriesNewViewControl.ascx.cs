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
    private int mItemsShown = 0;
    private int mCount = 5;

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
        try
        {
            if (!IsPostBack)
            {
                ServiceQueryOptions options = new ServiceQueryOptions(Count, 0);
                object[] args = { options };
                storiesView.DataSource = SessionManager.GetCachedCollection<TransitAccountStory>(StoryService, "GetLatestAccountStories", args);
                storiesView.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetDescription(string description)
    {
        if (mItemsShown++ >= 1)
            return string.Empty;

        if (string.IsNullOrEmpty(description))
            return string.Empty;

        return Renderer.GetSummary(description);
    }

    public string GetComments(int count)
    {
        if (count == 0) return "read";
        else if (count == 1) return "read one comment";
        else return string.Format("read {0} comments", count);
    }
}
