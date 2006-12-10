<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountPasswordReset.aspx.cs"
 Inherits="EmailAccountPasswordReset" Title="Your password has been reset" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Password Reset
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.Account.Name)); %></b>,
    </p>
    <p>
     Your password has been reset. Your temporary password is:
     <p style="margin: 10px;">
      <b><% Response.Write(Renderer.Render(Request.QueryString["Password"])); %></b>
     </p>
     You may now <a href="AccountLogin.aspx">log-in</a> with this new password. You will be asked to change it.
    </p>
    <p style="font-size: smaller;">
     If you have not reset your password or believe that this is an error, please 
     <a href="<% Response.Write(this.MailtoAdministrator); %>">notify the administrator</a> immediately.
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
