<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountGroupAccountInvitationReject.aspx.cs"
 Inherits="EmailAccountGroupAccountInvitationReject" Title="Your invitation has been declined" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Invitation Rejected
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.RequesterName)); %></b>,
    </p>
    <p>
     Sorry, but your friend 
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.AccountId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.AccountName)); %>
     </a>
     declined your invitation to join
     <a href="AccountGroupView.aspx?id=<% Response.Write(this.AccountGroupAccountInvitation.AccountGroupId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountInvitation.AccountGroupName)); %>
     </a>
     .
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
