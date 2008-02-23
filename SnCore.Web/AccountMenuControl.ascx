<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountMenuControl.ascx.cs" Inherits="AccountMenuControl" %>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountView.aspx">Preview Me</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a href="AccountInvitationsManage.aspx">Invite&nbsp;Friends</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/syndication.gif" /></td><td width="*"><a href="AccountFeedsManage.aspx">Syndication</a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountFeedWizard.aspx">Got a Blog?</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/blogs.gif" /></td><td width="*"><a href="AccountBlogsManage.aspx">Blogs</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/discussions.gif" /></td><td width="*"><a href="SystemDiscussionsManage.aspx">Discussions</a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a href="DiscussionsView.aspx">Post New</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/groups.gif" /></td><td width="*"><a href="AccountGroupsManage.aspx">Groups</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/inbox.gif" /></td><td width="*"><a runat="server" id="linkInbox" href="AccountMessageFoldersManage.aspx?folder=inbox">Inbox</a>
  <img src="images/account/star.gif" align="absmiddle" runat="server" id="linkRequestsStar" /></td></tr>
 <tr><td width="30" align="center"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountMessageFoldersManage.aspx?folder=sent">Sent</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/trash.gif" /></td><td width="*"><a href="AccountMessageFoldersManage.aspx?folder=trash">Trash</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a runat="server" id="linkRequests" href="AccountFriendRequestsManage.aspx">Requests</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/groups.gif" /></td><td width="*"><a runat="server" id="linkInvitations" href="AccountGroupAccountInvitationsManage.aspx">Invitations</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/flags.gif" /></td><td width="*"><a href="AccountFlagsManage.aspx">Abuse</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="AccountPreferencesManage.aspx">Preferences</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/subscription.gif" /></td><td width="*"><a href="AccountRssWatchsManage.aspx">Subscriptions</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/photos.gif" /></td><td width="*"><a href="AccountPicturesManage.aspx">Pictures</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/emails.gif" /></td><td width="*"><a href="AccountEmailsManage.aspx">E-Mails</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/websites.gif" /></td><td width="*"><a href="AccountWebsitesManage.aspx">Websites</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a href="AccountFriendsManage.aspx">Friends</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/favorites.gif" /></td><td width="*"><a href="AccountPlaceFavoritesManage.aspx">Favorites</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/queue.gif" /></td><td width="*"><a href="AccountPlaceQueueManage.aspx">Queue</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/places.gif" /></td><td width="*"><a href="AccountPlacesManage.aspx">Places</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/property.gif" /></td><td width="*"><a href="AccountPropertyManage.aspx">Property</a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a id="linkAccountPlaceRequests" runat="server" href="SystemAccountPlaceRequestsManage.aspx">Requests</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/events.gif" /></td><td width="*"><a href="AccountEventsManage.aspx">Events</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/openid.gif" /></td><td width="*"><a href="AccountOpenIdsManage.aspx">OpenId</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/license.gif" /></td><td width="*"><a href="AccountLicenseEdit.aspx">License</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/redirect.gif" /></td><td width="*"><a href="AccountRedirectsManage.aspx">Redirects</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/stories.gif" /></td><td width="*"><a href="AccountStoriesManage.aspx">Stories</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/surveys.gif" /></td><td width="*"><a href="AccountSurveysManage.aspx">Surveys</a></td></tr>
</table>
