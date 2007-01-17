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

    public TransitFeature Feature
    {
        get
        {
            if (mFeature == null)
            {
                mFeature = SessionManager.GetInstance<TransitFeature, string>(
                    "AccountEvent", SessionManager.ObjectService.GetLatestFeature);
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
                mAccountEvent = SessionManager.GetInstance<TransitAccountEvent, int, int>(
                    Feature.DataRowId, SessionManager.UtcOffset, SessionManager.EventService.GetAccountEventById);
            }

            return mAccountEvent;
        }
    }
}
