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
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountGroupPlacesViewControl : Control
{
    public int AccountGroupId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountGroupId", 0);
        }
        set
        {
            ViewState["AccountGroupId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        placesList.OnGetDataSource += new EventHandler(placesList_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            linkAll.Text = string.Format("&#187; {0} place{1}",
                placesList.VirtualItemCount, placesList.VirtualItemCount == 1 ? string.Empty : "s");
            linkAll.NavigateUrl = string.Format("AccountGroupPlacesView.aspx?id={0}", AccountGroupId);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        placesList.CurrentPageIndex = 0;
        placesList.VirtualItemCount = SessionManager.GroupService.GetAccountGroupPlacesCount(
            SessionManager.Ticket, AccountGroupId);
        placesList_OnGetDataSource(sender, e);
        placesList.DataBind();
        this.Visible = (placesList.VirtualItemCount > 0);
    }

    void placesList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = placesList.CurrentPageIndex;
        options.PageSize = placesList.PageSize;
        placesList.DataSource = SessionManager.GroupService.GetAccountGroupPlaces(
            SessionManager.Ticket, AccountGroupId, options);
        panelGrid.Update();
    }
}
