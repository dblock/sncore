<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountFriendRequestAccept.aspx.cs"
 Inherits="EmailAccountFriendRequestAccept" Title="Someone has accepted your friends request" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Friend Request Accepted
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountFriendRequest.AccountName)); %></b>,
    </p>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountFriendRequest.KeenId); %>">
      <% Response.Write(Renderer.Render(this.AccountFriendRequest.KeenName)); %>
     </a>
     is now your friend.
    </p>
    <asp:Panel ID="panelMessage" runat="server">
     <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
      <% Response.Write(Renderer.RenderEx(Request.QueryString["message"])); %>
     </div>
    </asp:Panel>
    <div>
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountFriendRequest.KeenId); %>">
      &#187; View <% Response.Write(this.AccountFriendRequest.KeenName); %>'s Profile
     </a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
