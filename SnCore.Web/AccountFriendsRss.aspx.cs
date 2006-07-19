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

public partial class AccountFriendsRss : AccountPersonPage
{
    public string Name
    {
        get
        {
            return Renderer.Render(string.Format("{0} {1}'s Friends",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), Account.Name));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                rssRepeater.DataSource = SocialService.GetFriendsActivityById(RequestId, null);
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
