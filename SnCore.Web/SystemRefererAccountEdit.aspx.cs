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
using SnCore.Tools.Drawing;
using System.IO;

public partial class SystemRefererAccountEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitRefererAccount t = StatsService.GetRefererAccountById(RequestId);
                    inputAccount.Text = t.AccountId.ToString();
                    inputRefererHost.Text = t.RefererHostName;
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
            TransitRefererAccount t = new TransitRefererAccount();
            t.RefererHostName = inputRefererHost.Text;
            t.AccountId = int.Parse(inputAccount.Text);
            t.Id = RequestId;
            StatsService.CreateOrUpdateRefererAccount(SessionManager.Ticket, t);
            Redirect("SystemRefererAccountsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
