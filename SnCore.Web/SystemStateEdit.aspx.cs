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
using SnCore.SiteMap;

public partial class SystemStateEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("States", Request, "SystemStatesManage.aspx"));

                int id = RequestId;

                object[] c_args = { null };
                inputCountry.DataSource = SessionManager.GetCachedCollection<TransitCountry>(
                    SessionManager.LocationService, "GetCountries", c_args);
                inputCountry.DataBind();

                if (id > 0)
                {
                    TransitState tw = SessionManager.LocationService.GetStateById(id);
                    inputName.Text = Renderer.Render(tw.Name);
                    inputCountry.Items.FindByValue(tw.Country).Selected = true;
                    sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New State", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
            TransitState tw = new TransitState();
            tw.Name = inputName.Text;
            tw.Id = RequestId;
            tw.Country = inputCountry.SelectedItem.Value;
            SessionManager.LocationService.AddState(SessionManager.Ticket, tw);
            Redirect("SystemStatesManage.aspx");

    }
}
