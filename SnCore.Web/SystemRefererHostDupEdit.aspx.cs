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

public partial class SystemRefererHostDupEdit : AuthenticatedPage
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
                    TransitRefererHostDup t = StatsService.GetRefererHostDupById(RequestId);
                    inputHost.Text = t.Host;
                    inputRefererHost.Text = t.RefererHost;
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
            TransitRefererHostDup t = new TransitRefererHostDup();
            t.Host = inputHost.Text;
            t.RefererHost = inputRefererHost.Text;
            t.Id = RequestId;
            StatsService.CreateOrUpdateRefererHostDup(SessionManager.Ticket, t);
            Redirect("SystemRefererHostDupsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
