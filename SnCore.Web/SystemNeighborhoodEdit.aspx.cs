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

public partial class SystemNeighborhoodEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Neighborhoods", Request, "SystemNeighborhoodsManage.aspx"));

                object[] c_args = { null };
                inputCountry.DataSource = SessionManager.GetCachedCollection<TransitCountry>(SessionManager.LocationService, "GetCountries", c_args);
                inputCountry.DataBind();

                if (RequestId > 0)
                {
                    TransitNeighborhood tc = SessionManager.LocationService.GetNeighborhoodById(RequestId);
                    inputName.Text = tc.Name;
                    inputCountry.Items.FindByValue(tc.Country).Selected = true;
                    inputCountry_SelectedIndexChanged(sender, e);
                    inputState.Items.FindByValue(tc.State).Selected = true;
                    inputState_SelectedIndexChanged(sender, e);
                    inputCity.Items.FindByValue(tc.City).Selected = true;
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
            object[] args = { inputCountry.SelectedValue, inputState.SelectedValue};
            inputCity.DataSource = SessionManager.GetCachedCollection<TransitCity>(SessionManager.LocationService, "GetCitiesByLocation", args);
            inputCity.DataBind();
    }


    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
            object[] args = { inputCountry.SelectedValue };
            inputState.DataSource = SessionManager.GetCachedCollection<TransitState>(SessionManager.LocationService, "GetStatesByCountry", args);
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
            SessionManager.LocationService.AddNeighborhood(SessionManager.Ticket, tc);
            Redirect("SystemNeighborhoodsManage.aspx");
    }

    public void mergeLookup_Click(object sender, EventArgs e)
    {
            gridMergeLookup.CurrentPageIndex = 0;
            gridMergeLookup.DataSource = SessionManager.LocationService.SearchNeighborhoodsByName(inputMergeWhat.Text);
            gridMergeLookup.DataBind();
    }

    public void gridMergeLookup_ItemCommand(object source, DataGridCommandEventArgs e)
    {
            switch (e.CommandName)
            {
                case "Merge":
                    int count = SessionManager.LocationService.MergeNeighborhoods(SessionManager.Ticket, 
                        RequestId, int.Parse(e.CommandArgument.ToString()));
                    ReportInfo(string.Format("Merged {0} records.", count));
                    mergeLookup_Click(source, e);
                    break;
            }
    }
}
