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
    public int AccountId
    {
        get
        {
            return RequestId > 0 ? RequestId : SessionManager.Account.Id;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                object[] args = { AccountId };
                TransitAccount ta = SessionManager.GetCachedItem<TransitAccount>(
                    AccountService, "GetAccountById", args);

                labelName.Text = string.Format("{0}'s Friends", Render(ta.Name));
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        object[] args = { AccountId };
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            SocialService, "GetFriendsActivityCountById", args);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            object[] args = { AccountId, options };
            gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountActivity>(
                SocialService, "GetFriendsActivityById", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
