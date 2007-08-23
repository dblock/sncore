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
using SnCore.Tools.Web;

public partial class EmailFeature : AuthenticatedPage
{
    private TransitFeature mFeature = null;
    private TransitAccount mAccount = null;

    public TransitFeature Feature
    {
        get
        {
            if (mFeature == null)
            {
                mFeature = SessionManager.ObjectService.GetFeatureById(
                    SessionManager.Ticket, RequestId);
            }
            return mFeature;
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = SessionManager.AccountService.GetAccountById(
                    SessionManager.Ticket, GetId("aid"));
            }
            return mAccount;
        }
    }

    public string GetDataObjectType()
    {
        switch (Feature.DataObjectName)
        {
            case "AccountFeed":
                return "syndicated blog";
            case "AccountFeedItem":
                return "syndicated blog post";
            case "Account":
                return "profile";
            case "AccountEvent":
                return "event";
            case "AccountGroup":
                return "group";
        }

        return Feature.DataObjectName.ToLower();
    }

    public string GetDataObjectName()
    {
        switch (Feature.DataObjectName)
        {
            case "AccountFeed":
                {
                    TransitAccountFeed t_instance = SessionManager.GetInstance<TransitAccountFeed, int>(
                        Feature.DataRowId, SessionManager.SyndicationService.GetAccountFeedById);
                    return t_instance.Name;
                }
            case "AccountFeedItem":
                {
                    TransitAccountFeedItem t_instance = SessionManager.GetInstance<TransitAccountFeedItem, int>(
                        Feature.DataRowId, SessionManager.SyndicationService.GetAccountFeedItemById);
                    return t_instance.Title;
                }
            case "Account":
                {
                    TransitAccount t_instance = SessionManager.GetInstance<TransitAccount, int>(
                        Feature.DataRowId, SessionManager.AccountService.GetAccountById);
                    return t_instance.Name;
                }
            case "AccountEvent":
                {
                    TransitAccountEvent t_instance = SessionManager.GetInstance<TransitAccountEvent, int, int>(
                        Feature.DataRowId, 0, SessionManager.EventService.GetAccountEventById);
                    return t_instance.Name;
                }
            case "AccountGroup":
                {
                    TransitAccountGroup t_instance = SessionManager.GetInstance<TransitAccountGroup, int>(
                        Feature.DataRowId, SessionManager.GroupService.GetAccountGroupById);
                    return t_instance.Name;
                }
            case "Place":
                {
                    TransitPlace t_instance = SessionManager.GetInstance<TransitPlace, int>(
                        Feature.DataRowId, SessionManager.PlaceService.GetPlaceById);
                    return t_instance.Name;
                }
        }

        return string.Empty;
    }
}

