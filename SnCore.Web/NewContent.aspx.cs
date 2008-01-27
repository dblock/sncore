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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;

[SiteMapDataAttribute("New Content")]
public partial class NewContent : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData();
        }
    }

    private void GetData()
    {
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = accounts.RepeatColumns;

        // accounts
        accounts.DataSource = SessionManager.GetCollection<TransitFeature, string>(
            "Account", serviceoptions, SessionManager.ObjectService.GetFeatures);
        accounts.DataBind();

        // places
        places.DataSource = SessionManager.GetCollection<TransitFeature, string>(
            "Place", serviceoptions, SessionManager.ObjectService.GetFeatures);
        places.DataBind();

        // feeds
        accountfeeds.DataSource = SessionManager.GetCollection<TransitFeature, string>(
            "AccountFeed", serviceoptions, SessionManager.ObjectService.GetFeatures);
        accountfeeds.DataBind();

        // events
        TransitAccountEventInstanceQueryOptions eventoptions = new TransitAccountEventInstanceQueryOptions();
        eventoptions.StartDateTime = DateTime.UtcNow;
        eventoptions.EndDateTime = DateTime.MaxValue;
        accountevents.DataSource = SessionManager.GetCollection<TransitAccountEventInstance, TransitAccountEventInstanceQueryOptions>(
            eventoptions, serviceoptions, SessionManager.EventService.GetAccountEventInstances);
        accountevents.DataBind();
    }

    public TransitAccount GetAccount(int id)
    {
        return SessionManager.GetInstance<TransitAccount, int>(
            id, SessionManager.AccountService.GetAccountById);
    }

    public TransitPlace GetPlace(int id)
    {
        return SessionManager.GetInstance<TransitPlace, int>(
            id, SessionManager.PlaceService.GetPlaceById);
    }

    public TransitAccountFeed GetAccountFeed(int id)
    {
        return SessionManager.GetInstance<TransitAccountFeed, int>(
            id, SessionManager.SyndicationService.GetAccountFeedById);
    }
}
