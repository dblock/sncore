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

public partial class AccountDiscussionThreadsRss : Page
{
    private string mWebsiteUrl = string.Empty;
    private TransitAccount mAccount = null;

    public string WebsiteUrl
    {
        get
        {
            if (string.IsNullOrEmpty(mWebsiteUrl))
            {
                mWebsiteUrl = SystemService.GetConfigurationByNameWithDefault(
                    "SnCore.WebSite.Url", "http://localhost/SnCoreWeb").Value;
            }

            return mWebsiteUrl;
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = AccountService.GetAccountById(RequestId);
            }
            return mAccount;
        }
    }

    public string RssTitle
    {
        get
        {
            return string.Format("{0}'s Discussion Posts", Renderer.Render(Account.Name));
        }
    }

    public string Link
    {
        get
        {
            return WebsiteUrl + string.Format("/AccountDiscussionThreadsView.aspx?id={0}&amp;toplevel={1}", RequestId, Request.Params["toplevel"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                DiscussionQueryOptions queryoptions = new DiscussionQueryOptions();
                queryoptions.AccountId = RequestId;
                queryoptions.TopOfThreadOnly = bool.Parse(Request.Params["toplevel"]);
                ServiceQueryOptions options = new ServiceQueryOptions();
                options.PageSize = 50;
                options.PageNumber = 0;
                rssRepeater.DataSource = DiscussionService.GetUserDiscussionThreads(queryoptions, options);
                rssRepeater.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}
