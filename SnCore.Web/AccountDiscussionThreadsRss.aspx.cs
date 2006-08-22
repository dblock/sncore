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
    private TransitAccount mAccount = null;

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                object[] args = { RequestId };
                mAccount = SessionManager.GetCachedItem<TransitAccount>(
                    AccountService, "GetAccountById", args);
            }
            return mAccount;
        }
    }

    public string RssTitle
    {
        get
        {
            return Renderer.Render(string.Format("{0} {1}' Discussion Posts",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), Account.Name));
        }
    }

    public string Link
    {
        get
        {
            return WebsiteUrl + string.Format("/AccountDiscussionThreadsView.aspx?id={0}&amp;toplevel={1}", RequestId, TopLevel);
        }
    }

    public bool TopLevel
    {
        get
        {
            object value = Request.Params["toplevel"];
            return (value == null ? false : bool.Parse(value.ToString()));
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
                queryoptions.TopOfThreadOnly = TopLevel;
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
