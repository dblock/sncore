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
using SnCore.Data.Hibernate;
using SnCore.WebControls;

public partial class SystemNeighborhoodEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Neighborhoods", Request, "SystemNeighborhoodsManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("Neighborhood");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

            string defaultcountry = SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            inputCountry.DataSource = SessionManager.GetCollection<TransitCountry, string>(
                defaultcountry, (ServiceQueryOptions)null, SessionManager.LocationService.GetCountriesWithDefault);
            inputCountry.DataBind();

            if (RequestId > 0)
            {
                TransitNeighborhood tc = SessionManager.LocationService.GetNeighborhoodById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = tc.Name;
                ListItemManager.TrySelect(inputCountry, tc.Country);
                inputCountry_SelectedIndexChanged(sender, e);
                ListItemManager.TrySelect(inputState, tc.State);
                inputState_SelectedIndexChanged(sender, e);
                ListItemManager.TrySelect(inputCity, tc.City);
                sitemapdata.Add(new SiteMapDataAttributeNode(tc.Name, Request.Url));
            }
            else
            {
                inputCountry_SelectedIndexChanged(sender, e);
                panelMerge.Visible = false;
                sitemapdata.Add(new SiteMapDataAttributeNode("New Neighborhood", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
        PageManager.SetDefaultButton(mergeLookup, panelMerge.Controls);
    }

    public void inputState_SelectedIndexChanged(object sender, EventArgs e)
    {
        inputCity.DataSource = SessionManager.GetCollection<TransitCity, string, string>(
            inputCountry.SelectedValue, inputState.SelectedValue, (ServiceQueryOptions) null, 
            SessionManager.LocationService.GetCitiesByLocation);
        inputCity.DataBind();
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        inputState.DataSource = SessionManager.GetCollection<TransitState, string>(
            inputCountry.SelectedValue, (ServiceQueryOptions) null, 
            SessionManager.LocationService.GetStatesByCountryName);
        inputState.DataBind();
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitNeighborhood tc = new TransitNeighborhood();
        tc.Name = inputName.Text;
        tc.Id = RequestId;
        tc.Country = inputCountry.SelectedValue;
        tc.State = inputState.SelectedValue;
        tc.City = inputCity.SelectedValue;
        if (string.IsNullOrEmpty(tc.City))
        {
            throw new Exception("City is required.");
        }
        SessionManager.CreateOrUpdate<TransitNeighborhood>(
            tc, SessionManager.LocationService.CreateOrUpdateNeighborhood);
        Redirect("SystemNeighborhoodsManage.aspx");
    }

    public void mergeLookup_Click(object sender, EventArgs e)
    {
        gridMergeLookup.CurrentPageIndex = 0;
        gridMergeLookup.DataSource = SessionManager.LocationService.SearchNeighborhoodsByName(
            SessionManager.Ticket, inputMergeWhat.Text, null);
        gridMergeLookup.DataBind();
    }

    public void gridMergeLookup_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "MergeThis":
                {
                    int count = SessionManager.LocationService.MergeNeighborhoods(SessionManager.Ticket,
                        RequestId, int.Parse(e.CommandArgument.ToString()));
                    ReportInfo(string.Format("Merged {0} records.", count));
                    mergeLookup_Click(source, e);
                }
                break;
            case "MergeTo":
                {
                    int count = SessionManager.LocationService.MergeNeighborhoods(SessionManager.Ticket,
                        int.Parse(e.CommandArgument.ToString()), RequestId);
                    Redirect("SystemNeighborhoodsManage.aspx");
                }
                break;
        }
    }
}
