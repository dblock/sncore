<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
 Inherits="_Default" Title="Welcome" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsNewView" Src="AccountsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacesNewView" Src="PlacesNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsActiveView" Src="AccountsActiveViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionPostsNewView" Src="DiscussionPostsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountStoriesNewView" Src="AccountStoriesNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedItemsNewView" Src="AccountFeedItemsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="FeedPreview" Src="AccountFeedPreviewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchDefault" Src="SearchDefaultControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeaturedView" Src="AccountFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceFeaturedView" Src="PlaceFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedFeaturedView" Src="AccountFeedFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BlogView" Src="AccountBlogViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountEventFeaturedView" Src="AccountEventFeaturedViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table width="100%" cellpadding="0" cellspacing="0">
  <tr>
   <td valign="top" width="75%">
    <SnCore:BlogView ID="websiteBlog" runat="server">
     <ContentLinkIds>
      <asp:ListItem>SnCore.PressContentGroup.Id</asp:ListItem>
      <asp:ListItem>SnCore.AddContentGroup.Id</asp:ListItem>
     </ContentLinkIds>
    </SnCore:BlogView>
   </td>
   <td valign="top" width="25%">
    <SnCore:SearchDefault runat="server" ID="searchDefault" />
    <SnCore:PlacesNewView ID="placesNewMain" runat="server" Count="2" />
    <SnCore:AccountsNewView ID="accountsNewMain" runat="server" Count="2" />
   </td>
  </tr>
 </table>
 <table width="100%" cellpadding="0" cellspacing="0"> 
  <tr>
   <td colspan="4">
    <div class="sncore_h2">
     <a href="Featured.aspx">Featured</a>
     <img src="images/site/right.gif" border="0" />
    </div>    
   </td>
  </tr>
  <tr>
   <td valign="top">
    <SnCore:AccountFeedFeaturedView ID="accountfeedFeatured" runat="server" />
   </td>
   <td valign="top">
    <SnCore:AccountFeaturedView ID="accountFeatured" runat="server" />
   </td>
   <td valign="top">
    <SnCore:PlaceFeaturedView ID="PlaceFeaturedView" runat="server" />
   </td>
   <td valign="top">
    <SnCore:AccountEventFeaturedView ID="accounteventsFeatured" runat="server" />
   </td>
  </tr>
 </table>
</asp:Content>
