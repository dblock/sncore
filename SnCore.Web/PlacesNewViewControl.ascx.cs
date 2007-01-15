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
using SnCore.WebServices;

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
            if (!IsPostBack)
            {
                ServiceQueryOptions options = new ServiceQueryOptions();
                options.PageNumber = 0;
                options.PageSize = Count;
                object[] args = { SessionManager.Ticket, options };
                Places.DataSource = SessionManager.GetCachedCollection<TransitPlace>(
                    SessionManager.PlaceService, "GetNewPlaces", args);
                Places.RepeatColumns = Count;
                Places.DataBind();
            }
    }
}
