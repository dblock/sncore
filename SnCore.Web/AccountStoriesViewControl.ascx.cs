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
using SnCore.Tools.Web;
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountStoriesViewControl : Control
{
    public int AccountId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountId", 0);
        }
        set
        {
            ViewState["AccountId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            accountStories.OnGetDataSource += new EventHandler(accountStories_OnGetDataSource);
            if (!IsPostBack)
            {
                accountStories_OnGetDataSource(sender, e);
                accountStories.DataBind();
                this.Visible = accountStories.Items.Count > 0;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void accountStories_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = accountStories.CurrentPageIndex;
        options.PageSize = accountStories.PageSize;
        AccountStoryQueryOptions queryoptions = new AccountStoryQueryOptions();
        queryoptions.PublishedOnly = true;
        object[] args = { AccountId, queryoptions, options };
        accountStories.DataSource = SessionManager.GetCachedCollection<TransitAccountStory>(
            StoryService, "GetAccountStoriesById", args);
    }

    public string GetDescription(object description)
    {
        return Renderer.RenderEx(Renderer.CleanHtml(description));
    }
}
