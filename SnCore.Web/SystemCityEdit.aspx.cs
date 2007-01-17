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

public partial class SystemCityEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Cities", Request, "SystemCitiesManage.aspx"));

            string defaultcountry = SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            inputCountry.DataSource = SessionManager.GetCollection<TransitCountry, string>(
                defaultcountry, (ServiceQueryOptions)null, SessionManager.LocationService.GetCountriesWithDefault);
            inputCountry.DataBind();

            if (RequestId > 0)
            {
                TransitCity tc = SessionManager.LocationService.GetCityById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = tc.Name;
                inputTag.Text = tc.Tag;
                inputCountry.Items.FindByValue(tc.Country).Selected = true;
                inputCountry_SelectedIndexChanged(sender, e);
                if (!string.IsNullOrEmpty(tc.State)) inputState.Items.FindByValue(tc.State).Selected = true;
                sitemapdata.Add(new SiteMapDataAttributeNode(tc.Name, Request.Url));
            }
            else
            {
                inputCountry_SelectedIndexChanged(sender, e);
                panelMerge.Visible = false;
                sitemapdata.Add(new SiteMapDataAttributeNode("New City", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
        PageManager.SetDefaultButton(mergeLookup, panelMerge.Controls);
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
        TransitCity tc = new TransitCity();
        tc.Name = inputName.Text;
        tc.Id = RequestId;
        tc.Tag = inputTag.Text;
        tc.Country = inputCountry.SelectedValue;
        tc.State = inputState.SelectedValue;
        if (string.IsNullOrEmpty(tc.State) && inputState.Items.Count > 0)
        {
            throw new Exception("State is required.");
        }
        SessionManager.LocationService.CreateOrUpdateCity(SessionManager.Ticket, tc);
        Redirect("SystemCitiesManage.aspx");
    }

    public void mergeLookup_Click(object sender, EventArgs e)
    {
        gridMergeLookup.CurrentPageIndex = 0;
        gridMergeLookup.DataSource = SessionManager.LocationService.SearchCitiesByName(
            SessionManager.Ticket, inputMergeWhat.Text, null);
        gridMergeLookup.DataBind();
    }

    public void gridMergeLookup_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Merge":
                int count = SessionManager.LocationService.MergeCities(SessionManager.Ticket,
                    RequestId, int.Parse(e.CommandArgument.ToString()));
                ReportInfo(string.Format("Merged {0} records.", count));
                mergeLookup_Click(source, e);
                break;
        }
    }
}
