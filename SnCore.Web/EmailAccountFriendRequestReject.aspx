<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountFriendRequestReject.aspx.cs"
 Inherits="EmailAccountFriendRequestReject" Title="Someone has rejected your friends request" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Friend Request Declined
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountFriendRequest.AccountName)); %></b>,
    </p>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(Renderer.Render(this.AccountFriendRequest.KeenId)); %>">
      <% Response.Write(Renderer.Render(this.AccountFriendRequest.KeenName)); %>
     </a>
     declined your friend request.
    </p>
    <asp:Panel ID="panelMessage" runat="server">
     <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
      <% Response.Write(Renderer.RenderEx(Request.QueryString["message"])); %>
     </div>
    </asp:Panel>
   </td>
  </tr>
 </table>
</asp:Content>
