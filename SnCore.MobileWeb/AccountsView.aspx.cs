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
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

[SiteMapDataAttribute("People")]
public partial class AccountsView : Page
{
    private LocationSelectorCountryStateCity mLocationSelector = null;

    public LocationSelectorCountryStateCity LocationSelector
    {
        get
        {
            if (mLocationSelector == null)
            {
                mLocationSelector = new LocationSelectorCountryStateCity(
                    this, true, inputCountry, inputState, inputCity);
            }

            return mLocationSelector;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            LocationSelector.SelectLocation(sender, new LocationEventArgs(Request));
            SetDefaultButton(search);
            GetData(sender, e);
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<
            SocialService.TransitAccountActivity, SocialService.ServiceQueryOptions, SocialService.AccountActivityQueryOptions>(
            GetQueryOptions(), SessionManager.SocialService.GetAccountActivityCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private SocialService.AccountActivityQueryOptions GetQueryOptions()
    {
        SocialService.AccountActivityQueryOptions options = new SocialService.AccountActivityQueryOptions();
        options.Name = inputSearch.Text;
        options.City = inputCity.Text;
        options.State = inputState.Text;
        options.Country = inputCountry.Text;
        return options;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        SocialService.AccountActivityQueryOptions options = GetQueryOptions();
        SocialService.ServiceQueryOptions serviceoptions = new SocialService.ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<
            SocialService.TransitAccountActivity, SocialService.ServiceQueryOptions, SocialService.AccountActivityQueryOptions>(
            options, serviceoptions, SessionManager.SocialService.GetAccountActivity);
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
    }

    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        Redirect(string.Format("AccountsView.aspx?{0}", e.CommandArgument));
    }
}
