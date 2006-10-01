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
using SnCore.Services;

public partial class AccountCreateInvitationWelcome : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                labelAccountName.Text = Render(SessionManager.Account.Name);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
