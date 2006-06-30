<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountView.aspx.cs"
 Inherits="AccountView" Title="Account | View" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="FriendsView" Src="AccountFriendsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SurveysView" Src="AccountSurveysViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="WebsitesView" Src="AccountWebsitesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="StoriesView" Src="AccountStoriesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="FeedsView" Src="AccountFeedsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BlogsView" Src="AccountBlogsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="ProfilesView" Src="AccountProfilesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacesView" Src="AccountPlacesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceFavoritesView" Src="AccountPlaceFavoritesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkAccount" Text="Account" runat="server" />
 </div>
 <asp:Panel CssClass="panel" ID="pnlAccount" runat="server">
  <table cellspacing="0" cellpadding="4" class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 100px;">
     <asp:Panel CssClass="sncore_nopicture_table" ID="accountNoPicture" runat="server" Visible="false">
      <img border="0" src="images/AccountThumbnail.gif" />
     </asp:Panel>
     <asp:DataList runat="server" ID="picturesView">
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <ItemTemplate>
       <a href="AccountPictureView.aspx?id=<%# Eval("Id").ToString() %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("Id").ToString() %>"
         alt="<%# base.Render(Eval("Name")) %>" />
         <div class="sncore_link_description">
         <%# ((int) Eval("CommentCount") >= 1) ? Eval("CommentCount").ToString() + 
          " comment" + (((int) Eval("CommentCount") == 1) ? "" : "s") : "" %>
         </div>
       </a>
      </ItemTemplate>
     </asp:DataList>
    </td>
    <td valign="top" width="*">
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td">
        <asp:Label CssClass="sncore_account_name" ID="accountName" runat="server" />
        <br />
        <asp:Label ID="accountLastLogin" CssClass="sncore_account_lastlogin" runat="server" />
        <br />
        <br />
        <asp:Label ID="accountCity" CssClass="sncore_account_locations" runat="server" />
        <asp:Label ID="accountState" CssClass="sncore_account_locations" runat="server" />
        <asp:Label ID="accountCountry" CssClass="sncore_account_locations" runat="server" />
        <br />
       </td>
       <td class="sncore_table_tr_td" valign="top" align="right">
        <asp:Label ID="accountId" CssClass="sncore_account_id" runat="server" />
       </td>
      </tr>
      <tr>
       <td colspan="2" class="sncore_table_tr_td" style="text-align: right;">
        <asp:HyperLink runat="server" ID="linkNewMessage" Text="&#187; Send Message" />
        <br />
        <asp:HyperLink runat="server" ID="linkAddToFriends" Text="&#187; Add to Friends" />
        <br />
        <asp:HyperLink Text="&#187; Discussion Posts" ID="linkDiscussionThreads" NavigateUrl="AccountDiscussionThreadsView.aspx"
         runat="server" />
        <asp:Panel ID="panelAdmin" runat="server">
         <div>
          <asp:LinkButton OnClick="impersonate_Click" runat="server" ID="linkImpersonate" Text="&#187; Impersonate" />
         </div>
         <div>
          <asp:LinkButton OnClick="promoteAdmin_Click" runat="server" ID="linkPromoteAdmin" Text="&#187; Promote to Admin" />
         </div>
         <div>
          <asp:LinkButton OnClick="demoteAdmin_Click" runat="server" ID="linkDemoteAdmin" Text="&#187; Demote from Admin" />
         </div>
         <div>
          <asp:LinkButton OnClick="feature_Click" runat="server" ID="linkFeature" Text="&#187; Feature" />
         </div>
         <div>
          <asp:LinkButton OnClick="deletefeature_Click" runat="server" ID="linkDeleteFeatures" Text="&#187; Delete Features" />
         </div>
         <div>
          <asp:HyperLink runat="server" ID="linkDelete" Text="&#187; Delete Account" />
         </div>
         <div>
          <asp:HyperLink runat="server" ID="linkResetPassword" Text="&#187; Reset Password" />
         </div>
        </asp:Panel>
       </td>
      </tr>
     </table>
     <SnCore:AccountReminder Style="width: 95%;" ID="accountReminder" runat="server" />
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
        socially bookmark this person:
       </td>
       <td class="sncore_table_tr_td">
        <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
       </td>
      </tr>
     </table>
     <SnCore:PlacesView runat="server" ID="placesView" />
     <SnCore:ProfilesView runat="server" ID="profilesView" />
     <SnCore:FriendsView runat="server" ID="friendsView" />
     <SnCore:StoriesView runat="server" ID="storiesView" />
     <SnCore:PlaceFavoritesView runat="server" ID="placeFavoritesView" />
     <SnCore:BlogsView runat="server" ID="blogsView" />
     <SnCore:FeedsView runat="server" ID="feedsView" />
     <SnCore:WebsitesView runat="server" ID="websitesView" />
     <SnCore:SurveysView runat="server" ID="surveysView" />
     <a name="Testimonials" />
     <SnCore:DiscussionFullView runat="server" ID="discussionTags" PostNewText="&#187; Post a Testimonial" />
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
