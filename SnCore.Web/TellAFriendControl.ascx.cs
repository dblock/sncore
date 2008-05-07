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

public partial class TellAFriendControl : Control
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkTellAFriend.NavigateUrl = string.Format("TellAFriend.aspx?Url={0}&Subject={1}",
                Renderer.UrlEncode(Request.Url.PathAndQuery),
                Renderer.UrlEncode(Page.Title));
        }
    }

    public void linkBroadcast_Click(object sender, EventArgs e)
    {
        // create a private audit entry for the broadcast draft
        TransitAccountAuditEntry audit_entry = new TransitAccountAuditEntry();
        audit_entry.AccountId = SessionManager.AccountId;
        audit_entry.Description = string.Format("[user:{0}] has broadcasted <a href='{1}'>{2}</a>", 
            SessionManager.AccountId, Request.Url.PathAndQuery, Renderer.Render(Page.Title));
        audit_entry.IsBroadcast = true;
        audit_entry.IsPrivate = false;
        audit_entry.IsSystem = false;
        audit_entry.Url = Request.Url.PathAndQuery;
        audit_entry.Id = SessionManager.SocialService.CreateOrUpdateAccountAuditEntry(
            SessionManager.Ticket, audit_entry);
        ReportInfo(string.Format("{0} has been broadcasted to your friends!", Page.Title));
    }
}
