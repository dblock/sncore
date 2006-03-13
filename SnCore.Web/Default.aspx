<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
 Inherits="_Default" Title="SnCore!" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsNewView" Src="AccountsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacesNewView" Src="PlacesNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsActiveView" Src="AccountsActiveViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionPostsNewView" Src="DiscussionPostsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountStoriesNewView" Src="AccountStoriesNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedItemsNewView" Src="AccountFeedItemsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TagWordsView" Src="TagWordsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="FeedPreview" Src="AccountFeedPreviewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchDefault" Src="SearchDefaultControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeaturedView" Src="AccountFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceFeaturedView" Src="PlaceFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedFeaturedView" Src="AccountFeedFeaturedViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <SnCore:AccountReminder ID="accountreminder" runat="server" />
 <table width="100%" cellpadding="0" cellspacing="0">
  <tr>
   <td valign="top" width="50%">
    <table class="sncore_half_table">
     <tr>
      <td class="sncore_table_tr_td">
       <asp:Label ID="labelDescription" runat="server" />
      </td>
     </tr>
    </table>
    <SnCore:SearchDefault runat="server" ID="searchDefault" />
    <SnCore:AccountFeaturedView ID="accountFeatured" runat="server" />
    <SnCore:PlaceFeaturedView ID="PlaceFeaturedView" runat="server" />
    <SnCore:TagWordsView ID="tagwordsView" runat="server" Count="100" />
    <SnCore:AccountsNewView ID="accountsNewMain" runat="server" Count="2" />
   </td>
   <td valign="top" width="50%">
    <SnCore:PlacesNewView ID="placesNewMain" runat="server" Count="2" />
    <SnCore:DiscussionPostsNewView ID="discussionsNewViewMain" runat="server" />
    <SnCore:AccountStoriesNewView ID="accountstoriesNewViewMain" Count="2" runat="server" />
    <SnCore:AccountFeedFeaturedView ID="accountfeedFeatured" runat="server" />
    <SnCore:AccountFeedItemsNewView ID="accountfeeditemsNewViewMain" Count="2" runat="server" />
    <table class="sncore_half_table">
     <tr>
      <td class="sncore_table_tr_td" align="center">
       <SnCore:BookmarksView ID="bookmarksViewMain" runat="server" RepeatColumns="3" />
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
