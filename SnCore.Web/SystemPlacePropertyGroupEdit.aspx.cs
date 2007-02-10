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

public partial class SystemPlacePropertyGroupEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridProperties.OnGetDataSource += new EventHandler(gridProperties_OnGetDataSource);
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Place Property Groups", Request, "SystemPlacePropertyGroupsManage.aspx"));

            if (RequestId > 0)
            {
                TransitPlacePropertyGroup t = SessionManager.PlaceService.GetPlacePropertyGroupById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                gridProperties_OnGetDataSource(this, null);
                gridProperties.DataBind();
                linkNewProperty.NavigateUrl = string.Format("SystemPlacePropertyEdit.aspx?pid={0}", RequestId);
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                panelProperties.Visible = false;
                sitemapdata.Add(new SiteMapDataAttributeNode("New Property Group", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitPlacePropertyGroup t = new TransitPlacePropertyGroup();
        t.Name = inputName.Text;
        t.Description = inputDescription.Text;
        t.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitPlacePropertyGroup>(
            t, SessionManager.PlaceService.CreateOrUpdatePlacePropertyGroup);
        Redirect("SystemPlacePropertyGroupsManage.aspx");
    }

    void gridProperties_OnGetDataSource(object sender, EventArgs e)
    {
        gridProperties.DataSource = SessionManager.PlaceService.GetPlaceProperties(
            SessionManager.Ticket, RequestId, null);
    }

    private enum Cells
    {
        id = 0
    };

    public void gridProperties_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                switch (e.CommandName)
                {
                    case "Delete":
                        SessionManager.Delete<TransitPlaceProperty>(id, SessionManager.PlaceService.DeletePlaceProperty);
                        ReportInfo("Place property deleted.");
                        gridProperties.CurrentPageIndex = 0;
                        gridProperties_OnGetDataSource(source, e);
                        gridProperties.DataBind();
                        break;
                }
                break;
        }
    }
}
