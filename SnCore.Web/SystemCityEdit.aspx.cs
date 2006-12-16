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
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Cities", Request, "SystemCitiesManage.aspx"));

                object[] c_args = { null };
                inputCountry.DataSource = SessionManager.GetCachedCollection<TransitCountry>(
                    SessionManager.LocationService, "GetCountries", c_args);
                inputCountry.DataBind();

                if (RequestId > 0)
                {
                    TransitCity tc = SessionManager.LocationService.GetCityById(RequestId);
                    inputName.Text = tc.Name;
                    inputTag.Text = tc.Tag;
                    inputCountry.Items.FindByValue(tc.Country).Selected = true;
                    inputCountry_SelectedIndexChanged(sender, e);
                    if (! string.IsNullOrEmpty(tc.State)) inputState.Items.FindByValue(tc.State).Selected = true;
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
            object[] args = { inputCountry.SelectedValue };
            inputState.DataSource = SessionManager.GetCachedCollection<TransitState>(SessionManager.LocationService, "GetStatesByCountry", args);
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
            SessionManager.LocationService.AddCity(SessionManager.Ticket, tc);
            Redirect("SystemCitiesManage.aspx");
    }

    public void mergeLookup_Click(object sender, EventArgs e)
    {
            gridMergeLookup.CurrentPageIndex = 0;
            gridMergeLookup.DataSource = SessionManager.LocationService.SearchCitiesByName(inputMergeWhat.Text);
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
