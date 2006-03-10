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
        accountStories.DataSource = StoryService.GetAccountStoriesById(AccountId);
    }

    public string GetSummary(string summary)
    {
        string result = SessionManager.ClearMarkups(summary);
        result = Renderer.RemoveHtml(result);
        if (result.Length > 256) result = result.Substring(0, 256) + " ...";
        return result;
    }
}
