<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountGroupAccountInvitationAccept.aspx.cs"
 Inherits="EmailAccountGroupAccountInvitationAccept" Title="Your invitation has been accepted" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Invitation Accepted
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.RequesterName)); %></b>,
    </p>
    <p>
     Your friend 
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.AccountId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.AccountName)); %>
     </a>
     accepted your invitation to join
     <a href="AccountGroupView.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.AccountGroupId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.AccountGroupName)); %>
     </a>
     !
     <% if (IsAdministratorApprovalRequired()) Response.Write("It still needs to be approved by the group administrator for your friend to join the group."); %>
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
