<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountEmailVerify.aspx.cs"
 Inherits="EmailAccountEmailVerify" Title="Please confirm your e-mail address" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  E-Mail Verification
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.Account.Name)); %></b>,
    </p>
    <p>
     The e-mail address 
     <b><% Response.Write(Renderer.Render(this.AccountEmailConfirmation.AccountEmail.Address)); %></b> 
     has been added to <a href='AccountView.aspx?id=<% Response.Write(this.Account.Id); %>'>your account</a>. You must activate it.
     <br />
     Please 
     <a href="AccountEmailVerify.aspx?id=<% Response.Write(RequestId); %>&code=<% Response.Write(this.AccountEmailConfirmation.Code); %>">
      click here
     </a>
     to confirm that this address is correct.
    </p>
    <p style="font-size: smaller;">
     If you have not added this e-mail to your account or believe that this is an error, please delete it from 
     <a href="AccountEmailsManage.aspx">your e-mail addresses</a> and <a href="<% Response.Write(this.MailtoAdministrator); %>">
      notify the administrator
     </a> immediately. You may also receive this notification as a reminder that you must confirm your e-mail address. 
     Accounts without a confirmed e-mail address are deleted after a month of inactivity.
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
