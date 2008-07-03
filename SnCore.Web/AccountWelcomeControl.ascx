<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountWelcomeControl.ascx.cs"
 Inherits="AccountWelcomeControl" %>
<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
   <asp:Panel ID="panelLoggedIn" runat="server">
    <div class="sncore_h2">
     Welcome
     <img src="images/site/right.gif" border="0" />
    </div>
    <div class="sncore_welcome">
     Welcome back
     <b><asp:Label runat="server" ID="loggedInAccountName" /></b>
     <a href="AccountManage.aspx">&#187; me me</a>
    </div>
    <div class="sncore_welcome">
     <table>
      <tr>
       <td width="30" align="center">
        <img src="images/account/inbox.gif" />
       </td>
       <td width="*">
        <a runat="server" id="linkInbox" href="AccountMessageFoldersManage.aspx?folder=inbox">
         <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Links, Inbox %>" /></a>
        <img src="images/account/star.gif" align="absmiddle" runat="server" id="linkRequestsStar" />
       </td>
      </tr>
      <tr>
       <td width="30" align="center">
        <img src="images/account/friends.gif" />
       </td>
       <td width="*">
        <a runat="server" id="linkRequests" href="AccountFriendRequestsManage.aspx">
         <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Links, Requests %>" /></a>
       </td>
      </tr>
      <tr>
       <td width="30" align="center">
        <img src="images/account/groups.gif" />
       </td>
       <td width="*">
        <a runat="server" id="linkInvitations" href="AccountGroupAccountInvitationsManage.aspx">
         <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Links, Invitations %>" /></a>
       </td>
      </tr>
     </table>
    </div>
   </asp:Panel>
   <asp:Panel ID="panelJoin" runat="server">
    <table cellpadding="0" cellspacing="0">
     <tr>
      <td width="*" align="left">
       <div class="sncore_h2">
        Welcome
        <img src="images/site/right.gif" border="0" />
       </div>
       <div class="sncore_welcome">
        Over
        <asp:HyperLink ID="panelJoinLinkAccounts" runat="server" NavigateUrl="AccountsView.aspx"
         Text="0" />
        people have a profile on
        <asp:Label ID="panelJoinWebsiteName" Text="this website" runat="server" />. <a href="AccountCreate.aspx">
         &#187; join</a>
       </div>
       <div class="sncore_welcome">
        Already a member? <a href="AccountLogin.aspx">&#187; log-in</a>
       </div>
      </td>
      <td>
       <a href="AccountCreate.aspx">
        <img src="images/buttons/join.gif" border="0" /></a>
      </td>
     </tr>
    </table>
   </asp:Panel>
  </td>
 </tr>
</table>
