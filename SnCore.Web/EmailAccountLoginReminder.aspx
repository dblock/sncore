<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountLoginReminder.aspx.cs"
 Inherits="EmailAccountLoginReminder" Title="You are missed" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  You Are Missed
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(Account.Name)); %></b>
    </p>
    <p>
     We noticed that you have not checked your account for a month. Your friends miss you. Please come back!
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
