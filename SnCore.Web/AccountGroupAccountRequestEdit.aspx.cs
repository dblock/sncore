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

public partial class AccountGroupAccountRequestEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (GroupId == 0)
            {
                throw new Exception("Missing Group");
            }

            TransitAccountGroup group = SessionManager.GroupService.GetAccountGroupById(
                SessionManager.Ticket, GroupId);
            linkAccountGroup.NavigateUrl = string.Format("AccountGroupView.aspx?id={0}", group.Id);
            linkBack.NavigateUrl = ReturnUrl;
            linkAccountGroup.Text = Renderer.Render(group.Name);
            imageAccountGroup.ImageUrl = string.Format("AccountGroupPictureThumbnail.aspx?id={0}", group.PictureId);
            inputMessage.Text = string.Format("Hi,\n\nI would like to join \"{0}\".\n\nThanks!\n", Renderer.Render(group.Name));

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(group.Name, Request, string.Format("AccountGroupView.aspx?id={0}", group.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Join", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageSave);

        if (!SessionManager.HasVerifiedEmailAddress())
        {
            ReportWarning("You don't have any verified e-mail addresses.\n" +
                "You must add/confirm a valid e-mail address before joining groups.");

            manageSave.Enabled = false;
        }
    }

    public string ReturnUrl
    {
        get
        {
            string result = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(result)) result = string.Format("AccountGroupView.aspx?id={0}", GroupId);
            return result;
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountGroupAccountRequest t_instance = new TransitAccountGroupAccountRequest();
        t_instance.Id = RequestId;
        t_instance.AccountId = SessionManager.Account.Id;
        t_instance.AccountGroupId = GroupId;
        t_instance.Message = inputMessage.Text;
        SessionManager.GroupService.CreateOrUpdateAccountGroupAccountRequest(
            SessionManager.Ticket, t_instance);
        Redirect(ReturnUrl);
    }

    public int GroupId
    {
        get
        {
            return GetId("gid");
        }
    }
}
