<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountGroupAccountRequest.aspx.cs"
 Inherits="EmailAccountGroupAccountRequest" Title="Request to join a group" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Membership Request
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountGroupAccountRequest.AccountId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountRequest.AccountName)); %>
     </a>
     wants to join
     "<a href="AccountGroupView.aspx?id=<% Response.Write(this.AccountGroupAccountRequest.AccountGroupId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountRequest.AccountGroupName)); %>
     </a>"
    </p>
    <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
     <% Response.Write(Renderer.RenderEx(this.AccountGroupAccountRequest.Message)); %>
    </div>
    <div>
     <a href="AccountGroupAccountRequestsManage.aspx?id=<% Response.Write(this.AccountGroupAccountRequest.AccountGroupId); %>">&#187; All Requests</a>
    </div>
    <div>
     <a href="AccountGroupAccountRequestsManage.aspx?id=<% Response.Write(this.AccountGroupAccountRequest.AccountGroupId); %>&rid=<% Response.Write(this.AccountGroupAccountRequest.Id); %>&action=Accept">&#187; Accept</a>
    </div>
    <div>
     <a href="AccountGroupAccountRequestsManage.aspx?id=<% Response.Write(this.AccountGroupAccountRequest.AccountGroupId); %>&rid=<% Response.Write(this.AccountGroupAccountRequest.Id); %>&action=Reject">&#187; Reject</a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
