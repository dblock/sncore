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

public partial class AccountPicturesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (RequestId == 0)
            {
                throw new Exception("Missing account.");
            }

            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                TransitAccount a = AccountService.GetAccountById(RequestId);
                linkAccount.Text = Renderer.Render(a.Name);
                linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", a.Id);
                this.Title = string.Format("{0}'s Pictures", Renderer.Render(a.Name));
        
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
        gridManage.VirtualItemCount = AccountService.GetAccountPicturesCountById(RequestId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageSize = gridManage.PageSize;
            options.PageNumber = gridManage.CurrentPageIndex;
            gridManage.DataSource = AccountService.GetAccountPicturesById(RequestId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
