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

    public string GetDataObjectName()
    {
        switch (Feature.DataObjectName)
        {
            case "AccountFeed":
                return "syndicated blog";
            case "Account":
                return "profile";
            case "AccountEvent":
                return "event";
        }

        return Feature.DataObjectName.ToLower();
    }
}

