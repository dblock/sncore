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

public partial class SystemAccountPropertyGroupEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
            gridProperties.OnGetDataSource += new EventHandler(gridProperties_OnGetDataSource);

            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Account Property Groups", Request, "SystemAccountPropertyGroupsManage.aspx"));

                if (RequestId > 0)
                {
                    TransitAccountPropertyGroup t = SessionManager.AccountService.GetAccountPropertyGroupById(RequestId);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    gridProperties_OnGetDataSource(this, null);
                    gridProperties.DataBind();
                    linkNewProperty.NavigateUrl = string.Format("SystemAccountPropertyEdit.aspx?pid={0}", RequestId);
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
            TransitAccountPropertyGroup t = new TransitAccountPropertyGroup();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.Id = RequestId;
            SessionManager.AccountService.CreateOrUpdateAccountPropertyGroup(SessionManager.Ticket, t);
            Redirect("SystemAccountPropertyGroupsManage.aspx");
    }

    void gridProperties_OnGetDataSource(object sender, EventArgs e)
    {
            gridProperties.DataSource = SessionManager.AccountService.GetAccountProperties(RequestId);
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
                            SessionManager.AccountService.DeleteAccountProperty(SessionManager.Ticket, id);
                            ReportInfo("Account property deleted.");
                            gridProperties.CurrentPageIndex = 0;
                            gridProperties_OnGetDataSource(source, e);
                            gridProperties.DataBind();
                            break;
                    }
                    break;
            }
    }
}
