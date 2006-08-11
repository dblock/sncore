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

public partial class SystemAccountPropertyGroupEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            gridProperties.OnGetDataSource += new EventHandler(gridProperties_OnGetDataSource);
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitAccountPropertyGroup t = AccountService.GetAccountPropertyGroupById(RequestId);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    gridProperties_OnGetDataSource(this, null);
                    gridProperties.DataBind();
                    linkNewProperty.NavigateUrl = string.Format("SystemAccountPropertyEdit.aspx?pid={0}", RequestId);
                }
                else
                {
                    panelProperties.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitAccountPropertyGroup t = new TransitAccountPropertyGroup();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.Id = RequestId;
            AccountService.CreateOrUpdateAccountPropertyGroup(SessionManager.Ticket, t);
            Redirect("SystemAccountPropertyGroupsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridProperties_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            gridProperties.DataSource = AccountService.GetAccountProperties(RequestId);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    };

    public void gridProperties_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
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
                            AccountService.DeleteAccountProperty(SessionManager.Ticket, id);
                            ReportInfo("Account property deleted.");
                            gridProperties.CurrentPageIndex = 0;
                            gridProperties_OnGetDataSource(source, e);
                            gridProperties.DataBind();
                            break;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
