<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountBlogPostView.aspx.cs"
 Inherits="AccountBlogPostView" Title="BlogPost" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="LicenseView" Src="AccountLicenseViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <!-- NOEMAIL-START -->
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccount" Text="Account" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccountBlog" Text="Blog" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkAccountBlogPost" Text="BlogPost"
   runat="server" />
 </div>
 <!-- NOEMAIL-END -->
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
     <asp:Label ID="BlogPostTitle" runat="server" />
    </div>
    <div class="sncore_h2sub">
     in
     <asp:HyperLink ID="BlogTitle" runat="server" />
     on
     <asp:Label ID="BlogPostCreated" runat="server" />
    </div>
    <!-- NOEMAIL-START -->
    <div class="sncore_h2sub">
     <a href="TellAFriend.aspx?Url=<% Response.Write(Renderer.UrlEncode(Request.Url.PathAndQuery)); %>&Subject=<% Response.Write(Renderer.UrlEncode(Title)); %>">&#187; Tell a Friend</a>     
    </div>
    <!-- NOEMAIL-END -->
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td">
    <asp:Label ID="BlogPostBody" runat="server" />
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
 <!-- NOEMAIL-START -->
 <a name="comments"></a>
 <SnCore:DiscussionFullView runat="server" ID="BlogPostComments" PostNewText="&#187; Post a Comment"
  Text="Comments" />
 <!-- NOEMAIL-END -->
</asp:Content>
