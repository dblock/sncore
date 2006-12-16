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

public partial class EmailAccountMessage : AuthenticatedPage
{
    private TransitAccountMessage mAccountMessage;

    public TransitAccountMessage AccountMessage
    {
        get
        {
            if (mAccountMessage == null)
            {
                mAccountMessage = SessionManager.AccountService.GetAccountMessageById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccountMessage;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("New message from {0}", Renderer.Render(AccountMessage.SenderAccountName));
    }
}

