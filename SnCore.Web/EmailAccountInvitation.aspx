<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountInvitation.aspx.cs"
 Inherits="EmailAccountInvitation" Title="Invitation to join" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Invitation to Join
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Hello,
    </p>
    <p>
     Your friend
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountInvitation.AccountId); %>">
      <% Response.Write(Renderer.Render(this.AccountInvitation.AccountName)); %>
     </a>
     has invited you to join
     <b><% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %></b>.
    </p>
    <asp:Panel ID="panelMessage" runat="server">
     <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
      <% Response.Write(Renderer.RenderEx(this.AccountInvitation.Message)); %>
     </div>
    </asp:Panel>
    <p>
     <a href="AccountCreateInvitation.aspx?id=<% Response.Write(this.AccountInvitation.Id); %>&code=<% Response.Write(Renderer.Render(this.AccountInvitation.Code)); %>">
      &#187; Accept Invitation
     </a>
     <a href="AccountDeclineInvitation.aspx?id=<% Response.Write(this.AccountInvitation.Id); %>&code=<% Response.Write(Renderer.Render(this.AccountInvitation.Code)); %>">
      &#187; Decline Invitation
     </a>
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
