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

public partial class AccountFriendsRss : AccountPersonPage
{
    public string PageTitle
    {
        get
        {
            if (Account == null)
            {
                return string.Empty;
            }

            return Renderer.Render(string.Format("{0} {1}'s Friends",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), Account.Name));
        }
    }

    public string PageDescription
    {
        get
        {
            if (Account == null)
            {
                return string.Empty;
            }

            return Renderer.Render(string.Format("{0}'s Friends", Account.Name));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Account == null)
            {
                Response.StatusCode = 404;
                Response.End();
                return;
            }

            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 10;
            rssRepeater.DataSource = SessionManager.SocialService.GetFriendsAccountActivity(
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
