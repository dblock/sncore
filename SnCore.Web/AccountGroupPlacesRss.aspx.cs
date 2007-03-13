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
using SnCore.Services;

public partial class AccountGroupPlacesRss : Page
{
    private TransitAccountGroup mAccountGroup;

    public TransitAccountGroup AccountGroup
    {
        get
        {
            if (mAccountGroup == null)
            {
                mAccountGroup = SessionManager.GroupService.GetAccountGroupById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccountGroup;
        }
        set
        {
            mAccountGroup = value;
        }
    }

    public string PageTitle
    {
        get
        {
            if (AccountGroup == null)
            {
                return string.Empty;
            }

            return Renderer.Render(string.Format("{0} {1}'s Places",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), AccountGroup.Name));
        }
    }

    public string PageDescription
    {
        get
        {
            if (AccountGroup == null)
            {
                return string.Empty;
            }

            return Renderer.Render(AccountGroup.Description);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (AccountGroup == null)
            {
                Response.StatusCode = 404;
                Response.End();
                return;
            }

            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            rssRepeater.DataSource = SessionManager.GroupService.GetAccountGroupPlaces(
                SessionManager.Ticket, RequestId, options);
            rssRepeater.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }

    public string Link
    {
        get
        {
            return WebsiteUrl.TrimEnd('/') + "/Default.aspx";
        }
    }

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }
}
