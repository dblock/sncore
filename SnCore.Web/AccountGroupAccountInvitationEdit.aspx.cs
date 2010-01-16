using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using System.Net.Mail;
using SnCore.Tools;
using System.Text.RegularExpressions;

public partial class AccountGroupAccountInvitationEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        friendsList.OnGetDataSource += new EventHandler(friendsList_OnGetDataSource);

        if (!IsPostBack)
        {
            linkBack.NavigateUrl = ReturnUrl;

            if (GroupId == 0)
            {
                panelInvite.Visible = false;
                throw new Exception("Missing Group");
            }

            if (GetAccountGroupAccount() == null && ! SessionManager.IsAdministrator)
            {
                panelInvite.Visible = false;
                throw new Exception("You must be a member of the group to invite friends.");
            }

            GetData(sender, e);

            TransitAccountGroup group = SessionManager.GroupService.GetAccountGroupById(
                SessionManager.Ticket, GroupId);
            linkAccountGroup.NavigateUrl = string.Format("AccountGroupView.aspx?id={0}", group.Id);
            linkAccountGroup.Text = Renderer.Render(group.Name);
            imageAccountGroup.ImageUrl = string.Format("AccountGroupPictureThumbnail.aspx?id={0}", group.PictureId);
            inputMessage.Text = string.Format("Hi,\n\nI would like to invite you to \"{0}\".\n\nJoin in!\n", Renderer.Render(group.Name));

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(group.Name, Request, string.Format("AccountGroupView.aspx?id={0}", group.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Invite a Friend", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void searchFriends_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
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

    public int GroupId
    {
        get
        {
            return GetId("gid");
        }
    }

    private TransitAccountFriendQueryOptions GetOptions()
    {
        TransitAccountFriendQueryOptions options = new TransitAccountFriendQueryOptions();
        options.AccountId = SessionManager.AccountId;
        options.Name = searchFriends.Text;
        return options;
    }

    void GetData(object sender, EventArgs e)
    {
        friendsList.CurrentPageIndex = 0;

        friendsList.VirtualItemCount = SessionManager.GetCount<TransitAccountFriend, TransitAccountFriendQueryOptions>(
            GetOptions(), SessionManager.SocialService.GetAccountFriendsCount);
        friendsList_OnGetDataSource(sender, e);
        friendsList.DataBind();

        if (friendsList.VirtualItemCount == 0)
        {
            ReportWarning(string.IsNullOrEmpty(searchFriends.Text) 
                ? "You have no friends."
                : string.Format("You don't have any friends matching '{0}'.", Renderer.Render(searchFriends.Text)));
        }
    }

    void friendsList_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = friendsList.CurrentPageIndex;
        options.PageSize = friendsList.PageSize;
        friendsList.DataSource = SessionManager.GetCollection<TransitAccountFriend, TransitAccountFriendQueryOptions>(
            GetOptions(), options, SessionManager.SocialService.GetAccountFriends);
        panelGrid.Update();
    }

    private TransitAccountGroupAccount GetAccountGroupAccount()
    {
        try
        {
            if (!SessionManager.IsLoggedIn)
                return null;

            return SessionManager.GroupService.GetAccountGroupAccountByAccountGroupId(
                SessionManager.Ticket, SessionManager.AccountId, GroupId);
        }
        catch
        {
        }

        return null;
    }

    public void friendsList_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Select":
                int id = int.Parse(e.CommandArgument.ToString());
                TransitAccountGroupAccountInvitation t_instance = new TransitAccountGroupAccountInvitation();
                t_instance.AccountId = id;
                t_instance.AccountGroupId = GroupId;
                t_instance.Message = inputMessage.Text;
                t_instance.RequesterId = SessionManager.Account.Id;
                SessionManager.GroupService.CreateOrUpdateAccountGroupAccountInvitation(
                    SessionManager.Ticket, t_instance);
                Redirect(ReturnUrl);
                break;
        }
    }

    public static Regex emailregex = new Regex(@".*@.*\..*", RegexOptions.Compiled);

    public void invite_Click(object sender, EventArgs e)
    {
        List<string> invitations = new List<string>();
        List<string> failures = new List<string>();
        ExceptionCollection exceptions = new ExceptionCollection();

        foreach (string email in inputEmailAddress.Text.Split(";,\n\r".ToCharArray()))
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email.Trim()))
                continue;

            try
            {
                // account is a member

                TransitAccount existing = SessionManager.AccountService.FindByEmail(
                    SessionManager.Ticket, email);

                TransitAccountGroupAccountInvitation t_instance = new TransitAccountGroupAccountInvitation();
                t_instance.AccountId = existing.Id;
                t_instance.AccountGroupId = GroupId;
                t_instance.Message = inputMessage.Text;
                t_instance.RequesterId = SessionManager.Account.Id;
                SessionManager.GroupService.CreateOrUpdateAccountGroupAccountInvitation(
                    SessionManager.Ticket, t_instance);
                continue;
            }
            catch
            {

            }

            try
            {
                MailAddress address = new MailAddress(email);
                TransitAccountInvitation invitation = new TransitAccountInvitation();
                invitation.Code = Guid.NewGuid().ToString();
                invitation.Email = email;
                invitation.Message = inputMessage.Text;
                invitation.AccountId = SessionManager.AccountId;
                invitation.AccountGroupId = GroupId;
                SessionManager.CreateOrUpdate<TransitAccountInvitation>(
                    invitation, SessionManager.AccountService.CreateOrUpdateAccountInvitation);
                invitations.Add(email);
            }
            catch (Exception ex)
            {
                failures.Add(email);
                exceptions.Add(new Exception(string.Format("Error inviting {0}: {1}", email, ex.Message), ex));
            }
        }

        if (invitations.Count > 0)
        {
            ReportInfo(string.Format("{0} invitation{1} sent",
                invitations.Count, invitations.Count == 1 ? string.Empty : "s"));
        }

        inputEmailAddress.Text = string.Join("\n", failures.ToArray());
        exceptions.Throw();
    }
}
