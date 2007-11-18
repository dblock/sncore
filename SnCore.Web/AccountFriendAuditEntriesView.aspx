<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountFriendAuditEntriesView.aspx.cs" Inherits="AccountFriendAuditEntriesView" Title="Friends Activity" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFriendAuditEntriesView" Src="AccountFriendAuditEntriesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <table width="100%">
  <tr>
   <td>
    <SnCore:Title ID="titleFriendAuditEntries" Text="Friends Activity" runat="server" ExpandedSize="100">  
     <Template>
      <div class="sncore_title_paragraph">
       Got friends? <a href="AccountsView.aspx">Find people</a> you want to be friends with and click
       on the <b>add to friends</b> link on their profile to send a friends request.
      </div>
     </Template>
    </SnCore:Title>
   </td>
   <td>
    <SnCore:RssLink ID="linkRelRss" runat="server" />
   </td>
  </tr>
 </table>
 <div class="sncore_h2sub">
  <a href="AccountFriendsManage.aspx">&#187; Manage Friends</a>
 </div>
 <SnCore:AccountFriendAuditEntriesView id="friendsView" runat="server" />
</asp:Content>
