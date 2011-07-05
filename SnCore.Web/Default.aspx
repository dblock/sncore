<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
 Inherits="_Default" Title="Food Social Network" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsNewView" Src="AccountsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacesNewView" Src="PlacesNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsActiveView" Src="AccountsActiveViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionPostsNewView" Src="DiscussionPostsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedItemsNewView" Src="AccountFeedItemsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="FeedPreview" Src="AccountFeedPreviewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchDefault" Src="SearchDefaultControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Featured" Src="FeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BlogView" Src="AccountBlogPreviewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedItemsFeaturedView" Src="AccountFeedItemsFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountGroupFeaturedView" Src="AccountGroupFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SelectCulture" Src="SelectCultureControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountWelcome" Src="AccountWelcomeControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table width="100%" cellpadding="0" cellspacing="0">
  <tr>
   <td valign="top" width="*">
    <SnCore:AccountFeedItemsFeaturedView id="featuredAccountFeedItems" runat="server" />
    <table cellspacing="0" cellpadding="4" class="sncore_half_table">
     <tr>
      <td align="center">
       <a href="Why.aspx"><img src="images/buttons/huh.gif" alt="What is this FoodCandy thing?" border="0" /></a>
       <a href="AccountCreate.aspx"><img src="images/buttons/join.gif" alt="Join - It's free!" border="0" /></a>
       <a href="DiscussionsView.aspx"><img src="images/buttons/discuss.gif" alt="Talk the talk with FoodCandy members." border="0" /></a>
       <a href="AccountFeedItemImgsView.aspx"><img src="images/buttons/porn.gif" alt="Don't tell your mother. It's food porn." border="0" /></a>
      </td>
     </tr>
    </table>
    <SnCore:BlogView ID="websiteBlog" runat="server" ItemsCollapseAfter="0" />
    <SnCore:AccountGroupFeaturedView id="featuredAccountGroup" runat="server" />
   </td>
   <td valign="top" width="333">
    <div id="panelRightFront">
     <SnCore:AccountWelcome ID="accountWelcome" runat="server" />
     <!-- NOEMAIL-START -->
     <SnCore:SearchDefault runat="server" ID="searchDefault" />
     <!-- NOEMAIL-END -->
     <SnCore:PlacesNewView ID="placesNewMain" runat="server" Count="2" />
     <SnCore:AccountsNewView ID="accountsNewMain" runat="server" Count="2" />
     <SnCore:DiscussionPostsNewView ID="discussionsNewMain" runat="server" HideIfOlder="5" />
    </div>
    <SnCore:Featured id="featured" runat="server" />
   </td>
  </tr>
 </table>
</asp:Content>
