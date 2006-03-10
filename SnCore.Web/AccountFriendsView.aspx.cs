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

public partial class AccountFriendsView : AccountPersonPage
{
    public void Page_Load()
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                gridManage.VirtualItemCount = SocialService.GetFriendsActivityCount(SessionManager.Ticket);
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();

                if (SessionManager.IsLoggedIn)
                {
                    linkNewFriends.NavigateUrl = string.Format("AccountsView.aspx?country={0}&state={1}&city={2}",
                        SessionManager.Account.Country, SessionManager.Account.State, SessionManager.Account.City);
                }
                else
                {
                    linkNewFriends.NavigateUrl = "AccountsView.aspx";
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = SocialService.GetFriendsActivity(SessionManager.Ticket, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
