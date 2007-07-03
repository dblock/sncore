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
using SnCore.Tools.Web;

public partial class EmailAccountFlag : AuthenticatedPage
{
    private TransitAccountFlag mAccountFlag;

    public TransitAccountFlag AccountFlag
    {
        get
        {
            if (mAccountFlag == null)
            {
                mAccountFlag = SessionManager.AccountService.GetAccountFlagById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccountFlag;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("{0} from {1}", 
            Renderer.Render(AccountFlag.AccountFlagType),
            Renderer.Render(AccountFlag.FlaggedAccountName));
    }
}

