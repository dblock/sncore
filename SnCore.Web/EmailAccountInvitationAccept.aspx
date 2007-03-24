<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountInvitationAccept.aspx.cs"
 Inherits="EmailAccountInvitationAccept" Title="Your invitation has been accepted" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Invitation Accepted
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountInvitation.AccountName)); %></b>,
    </p>
    <p>
     Your friend 
     <a href="AccountView.aspx?id=<% Response.Write(this.Account.Id); %>">
      <% Response.Write(Renderer.Render(this.Account.Name)); %>
     </a>
     (<a href="mailto:<% Response.Write(Renderer.Render(this.AccountInvitation.Email)); %>"><% 
      Response.Write(Renderer.Render(this.AccountInvitation.Email)); %></a>)
     accepted your invitation to join
     <b><% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %></b>
     !
    </p>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(this.Account.Id); %>">
      &#187; <% Response.Write(Renderer.Render(this.Account.Name)); %>'s Profile
     </a>
     <a href="AccountMessageEdit.aspx?id=<% Response.Write(this.Account.Id); %>">
      &#187; Send <% Response.Write(Renderer.Render(this.Account.Name)); %> a Welcome Message
     </a>
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
