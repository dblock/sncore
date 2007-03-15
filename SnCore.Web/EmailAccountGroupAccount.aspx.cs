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

public partial class EmailAccountGroupAccount : AuthenticatedPage
{
    private TransitAccountGroupAccount mAccountGroupAccount;

    public TransitAccountGroupAccount AccountGroupAccount
    {
        get
        {
            if (mAccountGroupAccount == null)
            {
                mAccountGroupAccount = SessionManager.GroupService.GetAccountGroupAccountById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccountGroupAccount;
        }
    }

    private TransitAccountGroup mAccountGroup;

    public TransitAccountGroup AccountGroup
    {
        get
        {
            if (mAccountGroup == null)
            {
                mAccountGroup = SessionManager.GroupService.GetAccountGroupById(
                    SessionManager.Ticket, AccountGroupAccount.AccountGroupId);
            }
            return mAccountGroup;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("Welcome to \"{0}\"", 
            Renderer.Render(AccountGroup.Name));
    }
}

