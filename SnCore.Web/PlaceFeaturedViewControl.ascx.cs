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

public partial class PlaceFeaturedViewControl : Control
{
    private TransitFeature mFeature = null;
    private TransitPlace mPlace = null;

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
                object[] args = { "Place" };
                mFeature = SessionManager.GetCachedItem<TransitFeature>(SystemService, "GetLatestFeature", args);
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
                object[] args = { Feature.DataRowId };
                mPlace = SessionManager.GetCachedItem<TransitPlace>(PlaceService, "GetPlaceById", args);
            }

            return mPlace;
        }
    }
}
