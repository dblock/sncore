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

public partial class AccountsRss : AccountPersonPage
{
    public string RssTitle
    {
        get
        {
            return Renderer.Render(string.Format("{0} People",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")));
        }
    }

    public string SortOrder
    {
        get
        {
            object o = Request.QueryString["sortorder"];
            return (o == null ? "LastLogin" : o.ToString());
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

    public bool Pictures
    {
        get
        {
            object o = Request.QueryString["pictures"];
            return (o == null ? false : bool.Parse(o.ToString()));
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

    public string Email
    {
        get
        {
            object o = Request.QueryString["email"];
            return (o == null ? string.Empty : o.ToString());
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                AccountActivityQueryOptions options = new AccountActivityQueryOptions();
                options.SortAscending = Ascending;
                options.SortOrder = SortOrder;
                options.PicturesOnly = Pictures;
                options.City = City;
                options.Country = Country;
                options.State = State;
                options.Name = Name;
                options.Email = Email;

                ServiceQueryOptions queryoptions = new ServiceQueryOptions();
                queryoptions.PageNumber = 0;
                queryoptions.PageSize = 25;

                object[] args = { options, queryoptions };
                rssRepeater.DataSource = SessionManager.GetCachedCollection<TransitAccountActivity>(
                    SocialService, "GetAccountActivity", args);
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
