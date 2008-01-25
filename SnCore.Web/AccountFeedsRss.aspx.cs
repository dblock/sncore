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

public partial class AccountFeedsRss : Page
{
    public string RssTitle
    {
        get
        {
            return Renderer.Render(string.Format("{0} Blog Directory",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountFeedQueryOptions options = new TransitAccountFeedQueryOptions();
            options.SortAscending = Ascending;
            options.SortOrder = SortOrder;
            options.City = City;
            options.Country = Country;
            options.State = State;
            options.Name = Name;

            ServiceQueryOptions queryoptions = new ServiceQueryOptions();
            queryoptions.PageNumber = 0;
            queryoptions.PageSize = 10;

            rssRepeater.DataSource = SessionManager.SyndicationService.GetAccountFeeds(
                SessionManager.Ticket, options, queryoptions);
            rssRepeater.DataBind();
        }
    }

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
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
            return new Uri(SessionManager.WebsiteUri, "AccountFeedsView.aspx").ToString();
        }
    }

    public string SortOrder
    {
        get
        {
            object o = Request.QueryString["sortorder"];
            return (o == null ? "Updated" : o.ToString());
        }
    }

    public bool Ascending
    {
        get
        {
            object o = Request.QueryString["asc"];
            return (o == null ? true : bool.Parse(o.ToString()));
        }
    }

    public string City
    {
        get
        {
            object o = Request.QueryString["city"];
            return (o == null ? string.Empty : o.ToString());
        }
    }

    public string Country
    {
        get
        {
            object o = Request.QueryString["country"];
            return (o == null ? string.Empty : o.ToString());
        }
    }

    public string State
    {
        get
        {
            object o = Request.QueryString["state"];
            return (o == null ? string.Empty : o.ToString());
        }
    }

    public string Name
    {
        get
        {
            object o = Request.QueryString["name"];
            return (o == null ? string.Empty : o.ToString());
        }
    }
}
