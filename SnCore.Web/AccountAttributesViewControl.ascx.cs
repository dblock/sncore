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
using SnCore.WebServices;

public partial class AccountAttributesViewControl : Control
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
        attributes.OnGetDataSource += new EventHandler(attributes_OnGetDataSource);
        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        attributes.CurrentPageIndex = 0;
        attributes.VirtualItemCount = SessionManager.GetCount<TransitAccountAttribute, int>(
            AccountId, SessionManager.AccountService.GetAccountAttributesCount);
        attributes_OnGetDataSource(sender, e);
        attributes.DataBind();
    }

    protected override void OnPreRender(EventArgs e)
    {
        this.Visible = attributes.Items.Count > 0;
        base.OnPreRender(e);
    }

    void attributes_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = attributes.CurrentPageIndex;
        options.PageSize = attributes.PageSize;
        attributes.DataSource = SessionManager.GetCollection<TransitAccountAttribute, int>(
            AccountId, options, SessionManager.AccountService.GetAccountAttributes);
    }
}
