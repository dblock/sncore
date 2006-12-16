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
                if (Feature != null && AccountEvent != null)
                {
                    linkFeature2.HRef = linkFeature3.HRef = string.Format("AccountEventView.aspx?id={0}", Feature.DataRowId);
                    labelFeatureName.Text = Render(AccountEvent.Name);
                    labelFeaturePlaceCity.Text = Render(AccountEvent.PlaceCity);
                    labelFeaturePlaceName.Text = Render(AccountEvent.PlaceName);
                    imgFeature.Src = string.Format("AccountEventPictureThumbnail.aspx?id={0}", AccountEvent.PictureId);
                }
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
                mFeature = SessionManager.GetCachedItem<TransitFeature>(
                    SessionManager.SystemService, "GetLatestFeature", args);
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
                object[] args = { SessionManager.Ticket, Feature.DataRowId, SessionManager.UtcOffset };
                mAccountEvent = SessionManager.GetCachedItem<TransitAccountEvent>(
                    SessionManager.EventService, "GetAccountEventById", args);
            }

            return mAccountEvent;
        }
    }
}
