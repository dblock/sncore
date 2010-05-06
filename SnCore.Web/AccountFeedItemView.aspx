<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemView.aspx.cs"
 Inherits="AccountFeedItemView" Title="FeedItem" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="LicenseView" Src="AccountLicenseViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="FacebookLike" Src="FacebookLikeControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td align="center" width="150">
    <a runat="server" id="linkAccountView" href="AccountView.aspx">
     <img border="0" src="AccountPictureThumbnail.aspx" runat="server" id="imageAccount" />
     <div>
      <asp:Label ID="labelAccountName" runat="server" />
     </div>
    </a>
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     <asp:HyperLink Target="_blank" ID="FeedItemTitle" runat="server" />
    </div>
    <div class="sncore_h2sub">
     &#187;
     <asp:HyperLink Target="_blank" ID="FeedXPosted" Text="x-posted" runat="server" />
     in
     <asp:HyperLink ID="FeedTitle" runat="server" />
     on
     <asp:Label ID="FeedItemCreated" runat="server" />
    </div>
    <div class="sncore_h2sub">
     <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
    </div>
    <div class="sncore_h2sub">
     <asp:UpdatePanel runat="server" ID="panelAdmin" UpdateMode="Conditional">
      <ContentTemplate>
       <asp:LinkButton OnClick="feature_Click" runat="server" ID="linkFeature" Text="Feature" />
       <asp:LinkButton OnClick="deletefeature_Click" runat="server" ID="linkDeleteFeatures" Text="&#187; Delete Features" />
       <asp:Label runat="server" ID="labelAccountFeedItemByAccountFeedIdFeature" />
      </ContentTemplate>
     </asp:UpdatePanel>
    </div>
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td">
    <asp:Label ID="FeedItemDescription" runat="server" />
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td>
    <SnCore:LicenseView runat="server" ID="licenseView" />       
   </td>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    bookmark:
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
   </td>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    <SnCore:FacebookLike ID="facebookLike" runat="server" />
   </td>
  </tr>
 </table>
 <!-- NOEMAIL-START -->
 <a name="comments"></a>
 <SnCore:DiscussionFullView runat="server" ID="FeedItemComments" OuterWidth="682" PostNewText="&#187; Post a Comment"
  Text="Comments" />
 <!-- NOEMAIL-END -->
</asp:Content>
