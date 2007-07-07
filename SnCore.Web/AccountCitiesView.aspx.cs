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
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Generic;
using nStuff.UpdateControls;
using System.Text;
using System.Collections.Specialized;

[SiteMapDataAttribute("Top Cities")]
public partial class AccountCitiesView : AccountPersonPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        IList<TransitAccountCity> cities = SessionManager.GetCollection<TransitAccountCity>(
            null, SessionManager.LocationService.GetAccountCities);
        gridManage.DataSource = cities;
        gridManage.DataBind();
        labelCount.Text = string.Format("{0} cities", cities.Count);
    }
}
