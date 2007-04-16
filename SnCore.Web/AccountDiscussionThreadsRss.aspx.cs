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
                mAccount = SessionManager.GetInstance<TransitAccount, int>(
                    RequestId, SessionManager.AccountService.GetAccountById);
            }
            return mAccount;
        }
    }

    public string RssTitle
    {
        get
        {
            TransitAccount acct = Account;
            if (acct == null) return string.Empty;
            return Renderer.Render(string.Format("{0} {1}' Discussion Posts",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), acct.Name));
        }
    }

    public string Link
    {
        get
        {
            return new Uri(SessionManager.WebsiteUri, string.Format("AccountDiscussionThreadsView.aspx?id={0}&amp;toplevel={1}", 
                RequestId, TopLevel)).ToString();
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
        if (!IsPostBack)
        {
            DiscussionQueryOptions queryoptions = new DiscussionQueryOptions();
            queryoptions.AccountId = RequestId;
            queryoptions.TopOfThreadOnly = TopLevel;
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageSize = 50;
            options.PageNumber = 0;
            rssRepeater.DataSource = SessionManager.DiscussionService.GetUserDiscussionThreads(
                SessionManager.Ticket, queryoptions, options);
            rssRepeater.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}
