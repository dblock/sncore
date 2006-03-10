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

public partial class AccountFriendRequestAct : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string action = Request.QueryString["action"];
                if (string.IsNullOrEmpty(action))
                {
                    throw new Exception("Missing action parameter.");
                }

                int id = RequestId;

                if (id == 0)
                {
                    throw new Exception("Missing id parameter.");
                }

                action = action.ToLower();

                switch (action)
                {
                    case "accept":
                        SocialService.AcceptAccountFriendRequest(SessionManager.Ticket, id, string.Empty);
                        break;
                    case "reject":
                        SocialService.RejectAccountFriendRequest(SessionManager.Ticket, id, string.Empty);
                        break;
                    default:
                        throw new Exception("Invalid action parameter.");
                }

                ReportInfo("Request " + action + "ed.");
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
