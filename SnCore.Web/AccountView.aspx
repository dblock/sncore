<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountView.aspx.cs"
 Inherits="AccountView" Title="Account | View" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

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
<%@ Register TagPrefix="SnCore" TagName="PropertyGroupsView" Src="AccountPropertyGroupsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacesView" Src="AccountPlacesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="GroupsView" Src="AccountGroupsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceFavoritesView" Src="AccountPlaceFavoritesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AttributesView" Src="AccountAttributesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="LicenseView" Src="AccountLicenseViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="CounterView" Src="CounterViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RedirectView" Src="AccountRedirectViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel CssClass="panel" ID="pnlAccount" runat="server">
  <table cellspacing="0" cellpadding="4" class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 100px;">
     <asp:Panel CssClass="sncore_nopicture_table" ID="accountNoPicture" runat="server" Visible="false">
      <img border="0" src="AccountPictureThumbnail.aspx" />
     </asp:Panel>
     <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="panelPictures">
      <ContentTemplate>
       <SnCoreWebControls:PagedList runat="server" ID="picturesView" RepeatColumns="1" RepeatRows="5" AllowCustomPaging="true">
        <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="&#187;"
         prevpagetext="&#171;" horizontalalign="Center" />
        <ItemTemplate>
         <a href='<%# string.Format("AccountPictureView.aspx?id={0}", Eval("Id")) %>'>
          <img border="0" src='<%# string.Format("AccountPictureThumbnail.aspx?id={0}", Eval("Id")) %>' alt='<%# base.Render(Eval("Name")) %>' />
          <div style="font-size: smaller;">
           <%# ((int) Eval("CommentCount") >= 1) ? Eval("CommentCount").ToString() + 
            " comment" + (((int) Eval("CommentCount") == 1) ? "" : "s") : "" %>
          </div>
         </a>
        </ItemTemplate>
       </SnCoreWebControls:PagedList>
      </ContentTemplate>
     </asp:UpdatePanel>
    </td>
    <td valign="top" width="*">
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td">
        <asp:Label CssClass="sncore_account_name" ID="accountName" runat="server" />
        <div>
         <SnCore:RedirectView id="redirect" runat="server" />
        </div>
        <!-- NOEMAIL-START -->
         <div class="sncore_description">
          last activity: <asp:Label ID="accountLastLogin" runat="server" />
         </div>
         <div class="sncore_description">
          profile views: <SnCore:CounterView ID="counterProfileViews" runat="server" />
         </div>
        <!-- NOEMAIL-END -->
        <div>
         <asp:Label ID="accountCity" CssClass="sncore_account_locations" runat="server" />
         <asp:Label ID="accountState" CssClass="sncore_account_locations" runat="server" />
        </div>
        <div>
         <asp:Label ID="accountCountry" CssClass="sncore_account_locations" runat="server" />
        </div>
       </td>
       <td class="sncore_table_tr_td" valign="top" align="right">
        <asp:Label ID="accountId" CssClass="sncore_account_id" runat="server" />
       </td>
      </tr>
      <tr>
       <td valign="top">
        <SnCore:AttributesView runat="server" ID="attributesView" />        
       </td>
       <td class="sncore_table_tr_td" style="text-align: right;">
        <div>
         <asp:HyperLink runat="server" ID="linkNewMessage" Text="&#187; Send Message" />
        </div>
        <div>
         <asp:HyperLink runat="server" ID="linkAddToFriends" Text="&#187; Add to Friends" />
        </div>
        <div>
         <a href="#Testimonials">Testimonials</a>
         <asp:HyperLink runat="server" ID="linkLeaveTestimonial" Text="&#187; Leave a Testimonial" />
        </div>
        <div>        
         <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
        </div>
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
          <asp:HyperLink runat="server" ID="linkAttributes" Text="&#187; Manage Attributes" />
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
       <td>
        <SnCore:LicenseView runat="server" ID="licenseView" />       
       </td>
       <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
        socially bookmark this person:
       </td>
       <td class="sncore_table_tr_td">
        <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
       </td>
      </tr>
     </table>
     <SnCore:PropertyGroupsView runat="server" ID="propertygroupsView" />
     <SnCore:FriendsView runat="server" ID="friendsView" />
     <SnCore:StoriesView runat="server" ID="storiesView" />
     <SnCore:PlaceFavoritesView runat="server" ID="placeFavoritesView" />
     <SnCore:BlogsView runat="server" ID="blogsView" />
     <SnCore:FeedsView runat="server" ID="feedsView" />
     <SnCore:WebsitesView runat="server" ID="websitesView" />
     <SnCore:PlacesView runat="server" ID="placesView" />
     <SnCore:GroupsView runat="server" ID="groupsView" />
     <SnCore:SurveysView runat="server" ID="surveysView" />
     <a name="Testimonials" />
     <SnCore:DiscussionFullView runat="server" ID="discussionTags" PostNewText="&#187; Leave a Testimonial" />
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
