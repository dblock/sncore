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
using System.Collections.Generic;
using SnCore.Tools.Web;

public partial class AccountEventFeaturedViewControl : Control
{
    private TransitFeature mFeature = null;
    private TransitAccountEvent mAccountEvent = null;

    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                panelFeatured.Visible = (Feature != null);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public TransitFeature Feature
    {
        get
        {
            if (mFeature == null)
            {
                object[] args = { "AccountEvent" };
                mFeature = SessionManager.GetCachedItem<TransitFeature>(SystemService, "GetLatestFeature", args);

                if (mFeature == null)
                    return null;
            }

            return mFeature;
        }
    }

    public TransitAccountEvent AccountEvent
    {
        get
        {
            if (mAccountEvent == null)
            {
                object[] args = { SessionManager.Ticket, Feature.DataRowId };
                mAccountEvent = SessionManager.GetCachedItem<TransitAccountEvent>(EventService, "GetAccountEventById", args);
            }

            return mAccountEvent;
        }
    }
}
