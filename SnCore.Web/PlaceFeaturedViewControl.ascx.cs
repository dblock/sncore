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
                mFeature = (TransitFeature)Cache["feature:Place"];
                if (mFeature == null)
                {
                    mFeature = SystemService.GetLatestFeature("Place");
                    if (mFeature == null)
                        return null;

                    Cache.Insert("feature:Place", mFeature, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
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
                mPlace = (TransitPlace) Cache[string.Format("Place:{0}", Feature.DataRowId)];
                if (mPlace == null)
                {
                    mPlace = PlaceService.GetPlaceById(Feature.DataRowId);
                    if (mPlace == null)
                        return null;

                    Cache.Insert(string.Format("Place:{0}", Feature.DataRowId), 
                        mPlace, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }

            return mPlace;
        }
    }
}
