<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountMenuControl.ascx.cs" Inherits="AccountMenuControl" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="AccountView.aspx">Preview</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a href="AccountInvitationsManage.aspx">Invite&nbsp;Friends</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/specials.gif" /></td><td width="*">
  <SnCore:AccountContentGroupLink ID="linkAddGroup" ShowLinkPrefix="false" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
 </td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/stories.gif" /></td><td width="*"><a href="AccountStoriesManage.aspx">Stories</a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountStoryEdit.aspx">Tell a Story</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/surveys.gif" /></td><td width="*"><a href="AccountSurveysManage.aspx">Surveys</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/syndication.gif" /></td><td width="*"><a href="AccountFeedsManage.aspx">Syndication</a></td></tr>
 <tr><td width="30" align="right"><img src="images/account/sent.gif" /></td><td width="*"><a href="AccountFeedWizard.aspx">Got a Blog?</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/blogs.gif" /></td><td width="*"><a href="AccountBlogsManage.aspx">Blogs</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/discussions.gif" /></td><td width="*"><a href="SystemDiscussionsManage.aspx">Forums</a></td></tr>
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
 <tr><td width="30" align="center"><img src="images/account/emails.gif" /></td><td width="*"><a href="AccountEmailsManage.aspx">E-Mails</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/openid.gif" /></td><td width="*"><a href="AccountOpenIdsManage.aspx">OpenId</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/websites.gif" /></td><td width="*"><a href="AccountWebsitesManage.aspx">Websites</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a href="AccountFriendsManage.aspx">Friends</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/favorites.gif" /></td><td width="*"><a href="AccountPlaceFavoritesManage.aspx">Favorites</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/places.gif" /></td><td width="*"><a href="AccountPlacesManage.aspx">Places</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/property.gif" /></td><td width="*"><a href="AccountPropertyManage.aspx">Property</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/events.gif" /></td><td width="*"><a href="AccountEventsManage.aspx">Events</a></td></tr>
</table>

<asp:Panel runat="server" ID="panelSystem">
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/contentgroups.gif" /></td><td width="*"><a href="AccountContentGroupsManage.aspx">Content</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemConfigurationsManage.aspx">Settings</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemSurveysManage.aspx">Surveys</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPicturesManage.aspx">Pictures</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPictureTypesManage.aspx">Pic. Types</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountEventTypesManage.aspx">Event&nbsp;Types</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="TagWordsManage.aspx">Tag&nbsp;Words</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemRemindersManage.aspx">Reminders</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="FeedTypesManage.aspx">Feed Types</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemBookmarksManage.aspx">Bookmarks</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemInvitationsManage.aspx">Invitations</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountPropertyGroupsManage.aspx">Property Groups</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAttributesManage.aspx">Attributes</a></td></tr>
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
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPlacePropertyGroupsManage.aspx">Property Groups</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/prefs.gif" /></td><td width="*"><a href="BugProjectsManage.aspx">Bugs</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugPrioritiesManage.aspx">Priorities</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugSeveritiesManage.aspx">Severities</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugResolutionsManage.aspx">Resolutions</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugStatusesManage.aspx">Statuses</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugTypesManage.aspx">Types</a></td></tr>
</table>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemStatsHits.aspx">Hit Stats</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemRefererHosts.aspx">Ref. Hosts</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemRefererQueries.aspx">Ref. Queries</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemRefererHostDupsManage.aspx">Host Dups</a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemRefererAccountsManage.aspx">Ref. Accounts</a></td></tr>
</table>
</asp:Panel>
