<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountLoginReminder.aspx.cs"
 Inherits="EmailAccountLoginReminder" Title="You are missed" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
  Misses You
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear 
     <a href='AccountView.aspx?id=<% Response.Write(Renderer.Render(this.Account.Id)); %>'>
      <% Response.Write(Renderer.Render(this.Account.Name)); %>
     </a>,
    </p>
    <p>
     We noticed that you have not checked your account for a very long time. Your friends at
     <a href='<% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost"))); %>'>
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
     </a>
     miss you. Please come back!
    </p>
    <p>
     If you do not wish to receive this e-mail, please take a minute to <a href="AccountDelete.aspx">delete your account</a>. 
     We apologize for the inconvenience. If you are having trouble logging in, please 
     <a href="<% Response.Write(this.MailtoAdministrator); %>">contact the administrator</a>.
    </p>
    <p>
     <a href="AccountLogin.aspx">&#187; Click Here to Log-In</a>
     <a href="AccountResetPassword.aspx">&#187; Forgot your Password?</a>
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
