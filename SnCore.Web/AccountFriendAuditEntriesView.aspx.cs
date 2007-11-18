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
using SnCore.SiteMap;

public partial class AccountFriendAuditEntriesView : AccountPersonPage
{
    public int RequestAccountId
    {
        get
        {
            return RequestId > 0 ? RequestId : 
                SessionManager.IsLoggedIn ? SessionManager.AccountId : 0;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (RequestAccountId == 0)
        {
            RedirectToLogin();
            return;
        }

        if (!IsPostBack)
        {
            linkRelRss.NavigateUrl = string.Format("AccountFriendAuditEntriesRss.aspx?id={0}", AccountId);
            friendsView.AccountId = RequestAccountId;

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Friends Activity", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }
}
