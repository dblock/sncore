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

public partial class AccountAddressEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Addresses", Request, "AccountAddressesManage.aspx"));

            TransitAccountAddress tw = null;

            int id = RequestId;

            if (id > 0)
            {
                tw = SessionManager.AccountService.GetAccountAddressById(SessionManager.Ticket, id);
                inputName.Text = Renderer.Render(tw.Name);
                inputApt.Text = Renderer.Render(tw.Apt);
                inputStreet.Text = Renderer.Render(tw.Street);
                inputCity.Text = Renderer.Render(tw.City);
                inputZip.Text = Renderer.Render(tw.Zip);
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Address", Request.Url));
            }

            StackSiteMap(sitemapdata);

            List<TransitCountry> countries = new List<TransitCountry>();
            if (tw == null || tw.Country.Length == 0) countries.Add(new TransitCountry());
            string defaultcountry = SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            countries.AddRange(SessionManager.GetCollection<TransitCountry, string>(
                defaultcountry, (ServiceQueryOptions)null, SessionManager.LocationService.GetCountriesWithDefault));

            List<TransitState> states = new List<TransitState>();
            if (tw == null || tw.State.Length == 0) states.Add(new TransitState());
            states.AddRange(SessionManager.GetCollection<TransitState>(
                (ServiceQueryOptions) null, SessionManager.LocationService.GetStates));

            inputCountry.DataSource = countries;
            inputCountry.DataBind();

            inputState.DataSource = states;
            inputState.DataBind();

            if (tw != null)
            {
                inputCountry.Items.FindByValue(tw.Country).Selected = true;
                inputState.Items.FindByValue(tw.State).Selected = true;
            }
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountAddress tw = new TransitAccountAddress();
        tw.Name = inputName.Text;
        tw.Street = inputStreet.Text;
        tw.Apt = inputApt.Text;
        tw.City = inputCity.Text;
        tw.Country = inputCountry.SelectedValue;
        tw.State = inputState.SelectedValue;
        tw.Zip = inputZip.Text;
        tw.Id = RequestId;
        SessionManager.AccountService.CreateOrUpdateAccountAddress(SessionManager.Ticket, tw);
        Redirect("AccountAddressesManage.aspx");
    }
}
