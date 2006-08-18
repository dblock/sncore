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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.Tools.Web;

public partial class AccountOpenIdsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {

                string openidmode = Request["openid.mode"];
                if (!string.IsNullOrEmpty(openidmode))
                {
                    NameValueCollectionSerializer serializer = new NameValueCollectionSerializer(Request.Params);
                    string consumerid = AccountService.VerifyOpenId(SessionManager.OpenIdToken, serializer.Names, serializer.Values);
                    TransitAccountOpenId to = new TransitAccountOpenId();
                    to.IdentityUrl = consumerid;
                    AccountService.AddAccountOpenId(SessionManager.Ticket, to);
                    Redirect(Request.Path);
                    return;
                }

                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = AccountService.GetAccountOpenIds(SessionManager.Ticket);
    }


    private enum Cells
    {
        id = 0
    };


    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.Item.Cells[(int) Cells.id].Text);
            switch (e.CommandName)
            {
                case "Delete":
                    AccountService.DeleteAccountOpenId(SessionManager.Ticket, id);
                    ReportInfo("OpenId deleted.");
                    gridManage.CurrentPageIndex = 0;
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
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
            TransitOpenIdRedirect r = AccountService.GetOpenIdRedirect(inputOpenIdIdentityUrl.Text, Request.Url.ToString());
            SessionManager.OpenIdToken = r.Token;
            Redirect(r.Url);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
