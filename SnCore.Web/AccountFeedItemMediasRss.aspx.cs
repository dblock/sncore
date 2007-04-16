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

public partial class AccountFeedItemMediasRss : AccountPersonPage
{
    public string Name
    {
        get
        {
            return Renderer.Render(string.Format("{0} Media",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountFeedItemMediaQueryOptions QueryOptions = new TransitAccountFeedItemMediaQueryOptions();
            QueryOptions.InterestingOnly = false;
            QueryOptions.VisibleOnly = true;

            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageNumber = 0;
            serviceoptions.PageSize = 10;

            rssRepeater.DataSource = SessionManager.GetCollection<TransitAccountFeedItemMedia, TransitAccountFeedItemMediaQueryOptions>(
                QueryOptions, serviceoptions, SessionManager.SyndicationService.GetAccountFeedItemMedias);
            rssRepeater.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}
