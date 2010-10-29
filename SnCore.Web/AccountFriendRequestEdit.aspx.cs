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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountFriendRequestEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ParentId != 0)
            {
                TransitAccount account = SessionManager.AccountService.GetAccountById(
                    SessionManager.Ticket, ParentId);
                linkKeen.NavigateUrl = string.Format("AccountView.aspx?id={0}", account.Id.ToString());
                linkBack.NavigateUrl = (string.IsNullOrEmpty(ReturnUrl)) ? linkKeen.NavigateUrl : ReturnUrl;
                linkKeen.Text = Renderer.Render(account.Name);
                imageKeen.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}", account.PictureId);
                inputMessage.Text = "Dear " + account.Name + "\n\nI would like to be your friend.\n\nThanks!\n";

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(account.Name, Request, string.Format("AccountView.aspx?id={0}", account.Id)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Friend Request", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }

        SetDefaultButton(manageAdd);

        if (!SessionManager.HasVerified())
        {
            ReportWarning("You don't have any verified e-mail addresses and/or profile photos.\n" +
                "You must add/confirm a valid e-mail address and upload a profile photo before making friends.");

            manageAdd.Enabled = false;
        }
    }

    public string ReturnUrl
    {
        get
        {
            string result = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(result)) result = string.Format("AccountView.aspx?id={0}", ParentId);
            return result;
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        SessionManager.SocialService.CreateOrUpdateAccountFriendRequest(
            SessionManager.Ticket, ParentId, inputMessage.Text);
        Redirect(ReturnUrl);
    }

    public int ParentId
    {
        get
        {
            return GetId("pid");
        }
    }
}
