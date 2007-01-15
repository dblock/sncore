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
using Wilco.Web.UI;
using SnCore.Services;
using System.Collections.Generic;
using System.Text;

public partial class AccountPropertyGroupsViewControl : Control
{
    public int AccountId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountId", 0);
        }
        set
        {
            ViewState["AccountId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object[] args = { SessionManager.Ticket, null };
            groups.DataSource = SessionManager.GetCachedCollection<TransitAccountPropertyGroup>(
                SessionManager.AccountService, "GetAccountPropertyGroups", args);
            groups.DataBind();
        }
    }

    public void groups_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
            case ListItemType.SelectedItem:
                DataGrid values = (DataGrid)e.Item.FindControl("values");
                TransitAccountPropertyGroup group = (TransitAccountPropertyGroup)e.Item.DataItem;
                if (group != null)
                {
                    object[] args = { SessionManager.Ticket, AccountId, group.Id, null };
                    List<TransitAccountPropertyValue> propertyvalues = SessionManager.GetCachedCollection<TransitAccountPropertyValue>(
                        SessionManager.AccountService, "GetAccountPropertyValues", args);
                    values.DataSource = propertyvalues;
                    HtmlControl title = (HtmlControl)e.Item.FindControl("title");
                    title.Visible = (propertyvalues.Count > 0);
                }
                break;
        }
    }
}
