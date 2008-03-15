<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountMenuControl.ascx.cs" Inherits="AccountMenuControl" %>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountView.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, PreviewMe %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a href="AccountInvitationsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, InviteFriends %>" /></a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/syndication.gif" /></td><td width="*"><a href="AccountFeedsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Syndication %>" /></a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountFeedWizard.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, GotABlog %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/blogs.gif" /></td><td width="*"><a href="AccountBlogsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Blogs %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/discussions.gif" /></td><td width="*"><a href="SystemDiscussionsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Discussions %>" /></a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a href="DiscussionsView.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, PostNew %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/groups.gif" /></td><td width="*"><a href="AccountGroupsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Groups %>" /></a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/inbox.gif" /></td><td width="*"><a runat="server" id="linkInbox" href="AccountMessageFoldersManage.aspx?folder=inbox"><asp:Literal runat="server" Text="<%$ Resources:Links, Inbox %>" /></a>
  <img src="images/account/star.gif" align="absmiddle" runat="server" id="linkRequestsStar" /></td></tr>
 <tr><td width="30" align="center"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountMessageFoldersManage.aspx?folder=sent"><asp:Literal runat="server" Text="<%$ Resources:Links, Sent %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/trash.gif" /></td><td width="*"><a href="AccountMessageFoldersManage.aspx?folder=trash"><asp:Literal runat="server" Text="<%$ Resources:Links, Trash %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a runat="server" id="linkRequests" href="AccountFriendRequestsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Requests %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/groups.gif" /></td><td width="*"><a runat="server" id="linkInvitations" href="AccountGroupAccountInvitationsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Invitations %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/flags.gif" /></td><td width="*"><a href="AccountFlagsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Abuse %>" /></a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="AccountPreferencesManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Preferences %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/subscription.gif" /></td><td width="*"><a href="AccountRssWatchsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Subscriptions %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/photos.gif" /></td><td width="*"><a href="AccountPicturesManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Pictures %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/emails.gif" /></td><td width="*"><a href="AccountEmailsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Emails %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/websites.gif" /></td><td width="*"><a href="AccountWebsitesManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Websites %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a href="AccountFriendsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Friends %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/favorites.gif" /></td><td width="*"><a href="AccountPlaceFavoritesManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Favorites %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/queue.gif" /></td><td width="*"><a href="AccountPlaceQueueManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Queue %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/places.gif" /></td><td width="*"><a href="AccountPlacesManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Places %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/property.gif" /></td><td width="*"><a href="AccountPropertyManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Property %>" /></a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a id="linkAccountPlaceRequests" runat="server" href="SystemAccountPlaceRequestsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Requests %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/events.gif" /></td><td width="*"><a href="AccountEventsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Events %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/openid.gif" /></td><td width="*"><a href="AccountOpenIdsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, OpenId %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/license.gif" /></td><td width="*"><a href="AccountLicenseEdit.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, License %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/redirect.gif" /></td><td width="*"><a href="AccountRedirectsManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Redirects %>" /></a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/stories.gif" /></td><td width="*"><a href="AccountStoriesManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Stories %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/surveys.gif" /></td><td width="*"><a href="AccountSurveysManage.aspx"><asp:Literal runat="server" Text="<%$ Resources:Links, Surveys %>" /></a></td></tr>
</table>
