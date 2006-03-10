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

public partial class PlacesNewViewControl : Control
{
    private int mCount = 2;

    public int Count
    {
        get
        {
            return mCount;
        }
        set
        {
            mCount = value;
        }
    }

    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                List<TransitPlace> items = (List<TransitPlace>)
                    Cache[string.Format("places:{0}", ClientID)];

                if (items == null)
                {
                    items = PlaceService.GetNewPlaces(Count);
                    Cache.Insert(string.Format("places:{0}", ClientID),
                        items, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }

                Places.RepeatColumns = Count;
                Places.DataSource = items;
                Places.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
