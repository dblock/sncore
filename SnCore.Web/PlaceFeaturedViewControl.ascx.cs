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
using SnCore.WebServices;

public partial class PlaceFeaturedViewControl : Control
{
    private TransitFeature mFeature = null;
    private TransitPlace mPlace = null;

    public void Page_Load()
    {
        if (!IsPostBack)
        {
            panelFeatured.Visible = (Feature != null);

            if (Feature != null && Place != null)
            {
                linkFeature2.HRef = linkFeature3.HRef = string.Format("PlaceView.aspx?id={0}", Feature.DataRowId);
                labelFeatureName.Text = Render(Place.Name);
                labelFeatureCity.Text = Render(Place.City);
                imgFeature.Src = string.Format("PlacePictureThumbnail.aspx?id={0}", Place.PictureId);
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
                    "Place", SessionManager.ObjectService.GetLatestFeature);
            }

            return mFeature;
        }
    }

    public TransitPlace Place
    {
        get
        {
            if (mPlace == null)
            {
                mPlace = SessionManager.GetInstance<TransitPlace, int>(
                    Feature.DataRowId, SessionManager.PlaceService.GetPlaceById);
            }

            return mPlace;
        }
    }
}
