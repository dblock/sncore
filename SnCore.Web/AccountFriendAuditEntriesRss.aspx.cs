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

public partial class AccountFriendAuditEntriesRss : Page
{
    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    public bool Broadcast
    {
        get
        {
            bool result = false;
            bool.TryParse(Request["Broadcast"], out result);
            return result;
        }
    }

    public string RssTitle
    {
        get
        {
            string title = Request["Title"];
            if (string.IsNullOrEmpty(title)) title = "Friends' Activity";
            return Renderer.Render(string.Format("{0} {1}",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), title));
        }
    }

    public string Link
    {
        get
        {
            return new Uri(SessionManager.WebsiteUri, "AccountFriendAuditEntriesView.aspx").ToString();
        }
    }

    TransitAccountAuditEntryQueryOptions GetQueryOptions()
    {
        TransitAccountAuditEntryQueryOptions qopt = new TransitAccountAuditEntryQueryOptions();
        qopt.AccountId = AccountId;
        qopt.Broadcast = Broadcast;
        qopt.System = false;
        qopt.Private = false;
        return qopt;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageSize = 50;
            options.PageNumber = 0;
            rssRepeater.DataSource = SessionManager.GetCollection<TransitAccountAuditEntry, TransitAccountAuditEntryQueryOptions>(
                GetQueryOptions(), options, SessionManager.SocialService.GetAccountFriendAuditEntries);
            rssRepeater.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }

    public int AccountId
    {
        get
        {
            return RequestId == 0 ? SessionManager.AccountId : RequestId;
        }
    }
}
