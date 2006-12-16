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

public partial class AccountFeedItemImgsRss : AccountPersonPage
{
    public string Name
    {
        get
        {
            return Renderer.Render(string.Format("{0} Images",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountFeedItemImgQueryOptions QueryOptions = new TransitAccountFeedItemImgQueryOptions();
            QueryOptions.InterestingOnly = false;
            QueryOptions.VisibleOnly = true;

            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageNumber = 0;
            serviceoptions.PageSize = 25;

            rssRepeater.DataSource = SessionManager.SyndicationService.GetAccountFeedItemImgs(QueryOptions, serviceoptions);
            rssRepeater.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}
