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

public partial class EmailBugClosed : AuthenticatedPage
{
    private TransitBug mBug;

    public TransitBug Bug
    {
        get
        {
            if (mBug == null)
            {
                mBug = SessionManager.BugService.GetBugById(
                    SessionManager.Ticket, RequestId);
            }
            return mBug;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("{0} #{1} has been resolved and closed.", Bug.Type, Bug.Id);
    }
}

