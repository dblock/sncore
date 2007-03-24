<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountInvitationReject.aspx.cs"
 Inherits="EmailAccountInvitationReject" Title="Your invitation has been rejected" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Invitation Rejected
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountInvitation.AccountName)); %></b>,
    </p>
    <p>
     Your friend 
     <a href="mailto:<% Response.Write(Renderer.Render(this.AccountInvitation.Email)); %>"><% 
      Response.Write(Renderer.Render(this.AccountInvitation.Email)); %></a>
     declined your invitation to join
     <b><% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %></b>
     !
    </p>
    <p>
     <a href="mailto:<% Response.Write(Renderer.Render(this.AccountInvitation.Email)); %>">
      &#187; E-Mail Your Friend
     </a>
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
