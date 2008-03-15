<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemPreferencesManage.aspx.cs"
 Inherits="SystemPreferencesManage" Title="System Preferences" %>
 
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <table class="sncore_account_table">
  <tr>
   <td valign="top" width="50%">
    <div class="sncore_h2">
     <asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:Links, SystemSettings %>" />
    </div>
    <table>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemConfigurationsManage.aspx"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Links, Settings %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemSurveysManage.aspx"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Links, Surveys %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPicturesManage.aspx"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Links, Pictures %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPictureTypesManage.aspx"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Links, PictureTypes %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountEventTypesManage.aspx"><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Links, EventTypes %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemRemindersManage.aspx"><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Links, Reminders %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="FeedTypesManage.aspx"><asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Links, FeedTypes %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemBookmarksManage.aspx"><asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Links, Bookmarks %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="AccountMadLibsManage.aspx"><asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:Links, MadLibs %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="TagWordsManage.aspx"><asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:Links, TagWords %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemInvitationsManage.aspx"><asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:Links, Invitations %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountPropertyGroupsManage.aspx"><asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:Links, PropertyGroups %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAttributesManage.aspx"><asp:Literal ID="Literal13" runat="server" Text="<%$ Resources:Links, Attributes %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountRedirectsManage.aspx"><asp:Literal ID="Literal14" runat="server" Text="<%$ Resources:Links, Redirects %>" /></a></td></tr>
    </table>
    <div class="sncore_h2">
     <asp:Literal ID="Literal15" runat="server" Text="<%$ Resources:Links, Content %>" />
    </div>
    <table>
     <tr><td width="30" align="center"><img src="images/account/content.gif" /></td><td width="*"><a href="NewContent.aspx"><asp:Literal ID="Literal17" runat="server" Text="<%$ Resources:Links, NewContent %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/property.gif" /></td><td width="*"><a href="MarketingCampaignsManage.aspx"><asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:Links, Campaigns %>" /></a></td></tr>
    </table>
    <div class="sncore_h2">
     <asp:Literal ID="Literal19" runat="server" Text="<%$ Resources:Links, Geography %>" />
    </div>
    <table>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemCountriesManage.aspx"><asp:Literal ID="Literal20" runat="server" Text="<%$ Resources:Links, Countries %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemStatesManage.aspx"><asp:Literal ID="Literal21" runat="server" Text="<%$ Resources:Links, States %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemCitiesManage.aspx"><asp:Literal ID="Literal22" runat="server" Text="<%$ Resources:Links, Cities %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemNeighborhoodsManage.aspx"><asp:Literal ID="Literal23" runat="server" Text="<%$ Resources:Links, Neighborhoods %>" /></a></td></tr>
    </table>
    <div class="sncore_h2">
     <asp:Literal ID="Literal24" runat="server" Text="<%$ Resources:Links, Runtime %>" />
    </div>
    <table>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemStatsCache.aspx"><asp:Literal ID="Literal25" runat="server" Text="<%$ Resources:Links, FrontEndCache %>" /></a></td></tr>
    </table>
   </td>
   <td valign="top" width="50%">
    <div class="sncore_h2">
     <asp:Literal ID="Literal26" runat="server" Text="<%$ Resources:Links, People %>" />
    </div>
    <table>
     <tr><td width="30" align="center"><img src="images/account/friends.gif" /></td><td width="*"><a href="SystemAccountsManage.aspx"><asp:Literal ID="Literal27" runat="server" Text="<%$ Resources:Links, Accounts %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/activity.gif" /></td><td width="*"><a href="AccountAuditEntriesView.aspx"><asp:Literal ID="Literal28" runat="server" Text="<%$ Resources:Links, Activity %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/flags.gif" /></td><td width="*"><a href="SystemAccountFlagsManage.aspx"><asp:Literal ID="Literal29" runat="server" Text="<%$ Resources:Links, ReportedAbuse %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountFlagTypesManage.aspx"><asp:Literal ID="Literal30" runat="server" Text="<%$ Resources:Links, FlagTypes %>" /></a></td></tr>
    </table>
    <div class="sncore_h2">
     <asp:Literal ID="Literal31" runat="server" Text="<%$ Resources:Links, Places %>" />
    </div>
    <table>
     <tr><td width="30" align="center"><img src="images/account/places.gif" /></td><td width="*"><a href="PlacesManage.aspx"><asp:Literal ID="Literal32" runat="server" Text="<%$ Resources:Links, Places %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="PlaceTypesManage.aspx"><asp:Literal ID="Literal33" runat="server" Text="<%$ Resources:Links, PlaceTypes %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountPlaceTypesManage.aspx"><asp:Literal ID="Literal34" runat="server" Text="<%$ Resources:Links, PropertyTypes %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemAccountPlaceRequestsManage.aspx?all=true"><asp:Literal ID="Literal35" runat="server" Text="<%$ Resources:Links, PropertyRequests %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPlacePropertyGroupsManage.aspx"><asp:Literal ID="Literal36" runat="server" Text="<%$ Resources:Links, PropertyGroups %>" /></a></td></tr>
    </table>
    <div class="sncore_h2">
     <asp:Literal ID="Literal37" runat="server" Text="<%$ Resources:Links, Bugs %>" />
    </div>
    <table>
     <tr><td width="30" align="center"><img src="images/account/prefs.gif" /></td><td width="*"><a href="BugProjectsManage.aspx"><asp:Literal ID="Literal38" runat="server" Text="<%$ Resources:Links, BugProjects %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugPrioritiesManage.aspx"><asp:Literal ID="Literal39" runat="server" Text="<%$ Resources:Links, Priorities %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugSeveritiesManage.aspx"><asp:Literal ID="Literal40" runat="server" Text="<%$ Resources:Links, Severities %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugResolutionsManage.aspx"><asp:Literal ID="Literal41" runat="server" Text="<%$ Resources:Links, Resolutions %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugStatusesManage.aspx"><asp:Literal ID="Literal42" runat="server" Text="<%$ Resources:Links, Statuses %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="BugTypesManage.aspx"><asp:Literal ID="Literal43" runat="server" Text="<%$ Resources:Links, Types %>" /></a></td></tr>
    </table>
    <div class="sncore_h2">
     <asp:Literal ID="Literal44" runat="server" Text="<%$ Resources:Links, Statistics %>" />
    </div>
    <table>
     <tr><td width="30" align="center"><img src="images/account/stats.gif" /></td><td width="*"><a href="SystemStatsHits.aspx"><asp:Literal ID="Literal45" runat="server" Text="<%$ Resources:Links, HitStats %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemRefererHosts.aspx"><asp:Literal ID="Literal46" runat="server" Text="<%$ Resources:Links, RefererHosts %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemRefererQueries.aspx"><asp:Literal ID="Literal47" runat="server" Text="<%$ Resources:Links, RefererQueries %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemRefererHostDupsManage.aspx"><asp:Literal ID="Literal48" runat="server" Text="<%$ Resources:Links, HostDups %>" /></a></td></tr>
     <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemRefererAccountsManage.aspx"><asp:Literal ID="Literal49" runat="server" Text="<%$ Resources:Links, RefererAccounts %>" /></a></td></tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
