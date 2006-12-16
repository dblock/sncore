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
using Wilco.Web.UI;
using SnCore.Services;
using System.Collections.Generic;
using System.Text;

public partial class PlaceNeighborhoodsViewControl : Control
{
    public EventHandler OnChange;

    public string City
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "City", string.Empty);
        }
        set
        {
            ViewState["City"] = value;
        }
    }

    public string Country
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "Country", string.Empty);
        }
        set
        {
            ViewState["Country"] = value;
        }
    }

    public string State
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "State", string.Empty);
        }
        set
        {
            ViewState["State"] = value;
        }
    }

    public string Neighborhood
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "Neighborhood", string.Empty);
        }
        set
        {
            ViewState["Neighborhood"] = value;
        }
    }

    public override void DataBind()
    {
        object[] args = { Country, State, City, null };
        values.DataSource = SessionManager.GetCachedCollection<TransitDistinctPlaceNeighborhood>(
            SessionManager.PlaceService, "GetPlaceNeighborhoods", args);
        values.DataBind();
        base.DataBind();
    }

    public void values_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (OnChange != null)
        {
            Neighborhood = e.CommandArgument.ToString();
            OnChange(source, e);
        }
    }
}
