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

public partial class EmailAccountGroupAccountRequest : AuthenticatedPage
{
    private TransitAccountGroupAccountRequest mAccountGroupAccountRequest;

    public TransitAccountGroupAccountRequest AccountGroupAccountRequest
    {
        get
        {
            if (mAccountGroupAccountRequest == null)
            {
                mAccountGroupAccountRequest = SessionManager.GroupService.GetAccountGroupAccountRequestById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccountGroupAccountRequest;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("{0} wants to join \"{1}\"", 
            Renderer.Render(AccountGroupAccountRequest.AccountName), 
            Renderer.Render(AccountGroupAccountRequest.AccountGroupName));
    }
}

