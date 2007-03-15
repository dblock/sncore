<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountGroupAccountInvitation.aspx.cs"
 Inherits="EmailAccountGroupAccountInvitation" Title="Request to join a group" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Membership Request
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear 
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.AccountId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.AccountName)); %>
     </a>,
    </p>
    <p>
     Your friend
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.RequesterId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.RequesterName)); %>
     </a>
     invites you to join
     "<a href="AccountGroupView.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.AccountGroupId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.AccountGroupName)); %>
     </a>"
    </p>
    <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
     <% Response.Write(Renderer.RenderEx(this.AccountGroupAccountInvitation.Message)); %>
    </div>
    <div>
     <a href="AccountGroupAccountInvitationsManage.aspx">&#187; All Requests</a>
    </div>
    <div>
     <a href="AccountGroupAccountInvitationsManage.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.AccountGroupId); %>&rid=<% Response.Write(this.AccountGroupAccountInvitation.Id); %>&action=Accept">&#187; Accept</a>
    </div>
    <div>
     <a href="AccountGroupAccountInvitationsManage.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.AccountGroupId); %>&rid=<% Response.Write(this.AccountGroupAccountInvitation.Id); %>&action=Reject">&#187; Reject</a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
