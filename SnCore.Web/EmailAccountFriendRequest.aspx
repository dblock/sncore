<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountFriendRequest.aspx.cs"
 Inherits="EmailAccountFriendRequest" Title="Someone wants to be your friend" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Friend Request
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountFriendRequest.KeenName)); %></b>,
    </p>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(Renderer.Render(this.AccountFriendRequest.AccountId)); %>">
      <% Response.Write(Renderer.Render(this.AccountFriendRequest.AccountName)); %>
     </a>
     wants to be your friend.
    </p>
    <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
     <% Response.Write(Renderer.RenderEx(this.AccountFriendRequest.Message)); %>
    </div>
    <div>
     <a href="AccountFriendRequestsManage.aspx">&#187; All Requests</a>
    </div>
    <div>
     <a href="AccountFriendRequestAct.aspx?id=<% Response.Write(this.AccountFriendRequest.Id); %>&action=accept">&#187; Accept</a>
    </div>
    <div>
     <a href="AccountFriendRequestAct.aspx?id=<% Response.Write(this.AccountFriendRequest.Id); %>&action=reject">&#187; Reject</a> (no message will be sent)
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
