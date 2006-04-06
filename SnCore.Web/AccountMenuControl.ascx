<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountMenuControl.ascx.cs" Inherits="AccountMenuControl" %>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountView.aspx">Preview</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountInvitationsManage.aspx">Invite&nbsp;Friends</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountStoriesManage.aspx">Stories</a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountStoryEdit.aspx">Tell a Story</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountSurveysManage.aspx">Surveys</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountFeedsManage.aspx">Syndication</a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountFeedWizard.aspx">Got a Blog?</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountBlogsManage.aspx">Blogs</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/inbox.gif" /></td><td width="*"><a runat="server" id="linkInbox" href="AccountMessageFoldersManage.aspx?folder=inbox">Inbox</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountMessageFoldersManage.aspx?folder=sent">Sent</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/trash.gif" /></td><td width="*"><a href="AccountMessageFoldersManage.aspx?folder=trash">Trash</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a runat="server" id="linkRequests" href="AccountFriendRequestsManage.aspx">Requests</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="AccountPreferencesManage.aspx">Preferences</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/photos.gif" /></td><td width="*"><a href="AccountPicturesManage.aspx">Pictures</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/prefs.gif" /></td><td width="*"><a href="AccountEmailsManage.aspx">E-Mails</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/openid.gif" /></td><td width="*"><a href="AccountOpenIdsManage.aspx">OpenId</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="AccountWebsitesManage.aspx">Websites</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a href="AccountFriendsManage.aspx">Friends</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/places.gif" /></td><td width="*"><a href="AccountPlaceFavoritesManage.aspx">Favorites</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/places.gif" /></td><td width="*"><a href="AccountPlacesManage.aspx">Places</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/events.gif" /></td><td width="*"><a href="AccountEventsManage.aspx">Events</a></td></tr>
</table>

<asp:Panel runat="server" ID="panelSystem">
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemConfigurationsManage.aspx">Settings</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemDiscussionsManage.aspx">Discussions</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemSurveysManage.aspx">Surveys</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPicturesManage.aspx">Pictures</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPictureTypesManage.aspx">Pic. Types</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountEventTypesManage.aspx">Event&nbsp;Types</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="TagWordsManage.aspx">Tag&nbsp;Words</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemRemindersManage.aspx">Reminders</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemInvitationsManage.aspx">Invitations</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="FeedTypesManage.aspx">Feed Types</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemBookmarksManage.aspx">Bookmarks</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemCountriesManage.aspx">Countries</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemStatesManage.aspx">States</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemCitiesManage.aspx">Cities</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="PlacesManage.aspx">Places</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="PlaceTypesManage.aspx">Place Types</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountPlaceTypesManage.aspx">Rel. Types</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountPlaceRequestsManage.aspx">Rel. Requests</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/prefs.gif" /></td><td width="*"><a href="BugProjectsManage.aspx">Bugs</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugPrioritiesManage.aspx">Priorities</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugSeveritiesManage.aspx">Severities</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugResolutionsManage.aspx">Resolutions</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugStatusesManage.aspx">Statuses</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugTypesManage.aspx">Types</a></td></tr>
</table>
</asp:Panel>
