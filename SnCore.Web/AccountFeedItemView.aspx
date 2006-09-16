<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemView.aspx.cs"
 Inherits="AccountFeedItemView" Title="FeedItem" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="LicenseView" Src="AccountLicenseViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccount" Text="Account" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccountFeed" Text="Feed" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkAccountFeedItem" Text="FeedItem"
   runat="server" />
 </div>
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td align="center" width="150">
    <a runat="server" id="linkAccountView" href="AccountView.aspx">
     <img border="0" src="images/AccountThumbnail.gif" runat="server" id="imageAccount" />
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
     <a href="TellAFriend.aspx?Url=<% Response.Write(Renderer.UrlEncode(Request.Url.PathAndQuery)); %>&Subject=<% Response.Write(Renderer.UrlEncode(Title)); %>">&#187; Tell a Friend</a>     
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
    socially bookmark this entry:
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
   </td>
  </tr>
 </table>
 <a name="comments"></a>
 <SnCore:DiscussionFullView runat="server" ID="FeedItemComments" PostNewText="&#187; Post a Comment"
  Text="Comments" />
</asp:Content>
