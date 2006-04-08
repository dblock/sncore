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
                mFeature = (TransitFeature)Cache["feature:accountevent"];
                if (mFeature == null)
                {
                    mFeature = SystemService.GetLatestFeature("AccountEvent");
                    if (mFeature == null)
                        return null;

                    Cache.Insert("feature:accountevent", mFeature, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
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
                mAccountEvent = (TransitAccountEvent) Cache[string.Format("accountevent:{0}", Feature.DataRowId)];
                if (mAccountEvent == null)
                {
                    mAccountEvent = EventService.GetAccountEventById(SessionManager.Ticket, Feature.DataRowId);
                    if (mAccountEvent == null)
                        return null;

                    Cache.Insert(string.Format("accountevent:{0}", Feature.DataRowId), 
                        mAccountEvent, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }

            return mAccountEvent;
        }
    }
}
