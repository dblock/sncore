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
    private DiscussionQueryOptions mOptions = null;

    public DiscussionQueryOptions QueryOptions
    {
        get
        {
            if (mOptions == null)
            {
                mOptions = new DiscussionQueryOptions();
                mOptions.AccountId = AccountId;
                mOptions.TopOfThreadOnly = showTopLevel.Checked;
            }
            return mOptions;
        }
    }

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

    void GetData(object sender, EventArgs e)
    {
        discussionThreadView.CurrentPageIndex = 0;
        discussionThreadView.VirtualItemCount = DiscussionService.GetUserDiscussionThreadsCount(QueryOptions);
        gridManage_OnGetDataSource(sender, e);
        discussionThreadView.DataBind();
    }
 
    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = discussionThreadView.CurrentPageIndex;
            options.PageSize = discussionThreadView.PageSize;
            discussionThreadView.DataSource = DiscussionService.GetUserDiscussionThreads(QueryOptions, options);
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
            GetData(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        discussionThreadView.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        try
        {
            if (!IsPostBack)
            {
                object[] args = { AccountId };
                TransitAccount ta = SessionManager.GetCachedItem<TransitAccount>(
                    AccountService, "GetAccountById", args);

                linkAccount.Text = Renderer.Render(ta.Name);
                linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", ta.Id);
                this.Title = labelHeader.Text = string.Format("{0}'s Discussion Posts", Renderer.Render(ta.Name));
                
                GetData(sender, e);
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
