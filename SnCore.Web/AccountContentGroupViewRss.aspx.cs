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
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;

public partial class AccountContentGroupRss : Page
{
    private TransitAccountContentGroup mContentGroup = null;

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    public string Name
    {
        get
        {
            return Renderer.Render(string.Format("{0} {1}",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), ContentGroup.Name));
        }
    }

    public string Link
    {
        get
        {
            return WebsiteUrl.TrimEnd('/') + "/Default.aspx";
        }
    }

    public TransitAccountContentGroup ContentGroup
    {
        get
        {
            if (mContentGroup == null)
            {
                mContentGroup = SessionManager.ContentService.GetAccountContentGroupById(
                    SessionManager.Ticket, RequestId);
            }

            return mContentGroup;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            rssRepeater.DataSource = SessionManager.ContentService.GetAccountContents(
                SessionManager.Ticket, RequestId, options);
            rssRepeater.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}
