<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFriendsView.aspx.cs" Inherits="AccountFriendsView" Title="Friends" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel UpdateMode="Conditional" ID="panelLinks" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       <asp:Label id="labelName" runat="server" />
      </div>
      <div class="sncore_h2sub">
       <asp:HyperLink ID="linkAccount" runat="server" Text="&#187; Back" />
       <a href="AccountsView.aspx">&#187; All People</a>
       <a href="AccountFriendAuditEntriesView.aspx">&#187; Friends Activity</a>
       <a href="AccountInvitationsManage.aspx">&#187; Invite a Friend</a>
       <a href="RefererAccountsView.aspx">&#187; Top Traffickers</a>
      </div>
     </td>
     <td>
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" />
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" AllowCustomPaging="true" RepeatColumns="4" RepeatRows="3" 
    RepeatDirection="Horizontal" CssClass="sncore_table" ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <a href="AccountView.aspx?id=<%# Eval("FriendId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("FriendPictureId") %>" />
     </a>
     <div>
      <a href="AccountView.aspx?id=<%# Eval("FriendId") %>">
       <%# base.Render(Eval("FriendName")) %>
      </a>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList> 
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
