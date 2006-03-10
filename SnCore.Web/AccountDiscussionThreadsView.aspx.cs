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
using SnCore.Services;
using SnCore.WebServices;

public partial class AcountDiscussionThreadsView : Page
{
    public int AccountId
    {
        get
        {
            if (RequestId != 0)
                return RequestId;

            if (SessionManager.IsLoggedIn)
                return SessionManager.Account.Id;

            return 0;
        }
    }

    void GetData()
    {
        DiscussionQueryOptions queryoptions = new DiscussionQueryOptions();
        queryoptions.AccountId = AccountId;
        queryoptions.TopOfThreadOnly = showTopLevel.Checked;
        discussionView.CurrentPageIndex = 0;
        discussionView.VirtualItemCount = DiscussionService.GetUserDiscussionThreadsCount(queryoptions);
        gridManage_OnGetDataSource(this, null);
        discussionView.DataBind();
    }

   
    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            DiscussionQueryOptions queryoptions = new DiscussionQueryOptions();
            queryoptions.AccountId = AccountId;
            queryoptions.TopOfThreadOnly = showTopLevel.Checked;
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = discussionView.CurrentPageIndex;
            options.PageSize = discussionView.PageSize;
            discussionView.DataSource = DiscussionService.GetUserDiscussionThreads(queryoptions, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void showTopLevel_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GetData();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        discussionView.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        try
        {
            if (!IsPostBack)
            {
                TransitAccount ta = AccountService.GetAccountById(AccountId);
                linkAccount.Text = Renderer.Render(ta.Name);
                linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", ta.Id);
                this.Title = labelHeader.Text = string.Format("{0}'s Discussion Posts", Renderer.Render(ta.Name));
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        linkRss.NavigateUrl = string.Format("AccountDiscussionThreadsRss.aspx?id={0}&toplevel={1}", 
            AccountId, showTopLevel.Checked);
        base.OnPreRender(e);
    }
}
