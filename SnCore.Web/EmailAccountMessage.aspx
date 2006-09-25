<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountMessage.aspx.cs"
 Inherits="EmailAccountMessage" Title="Message" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  New Message
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountMessage.RecepientAccountName)); %></b>,
    </p>
    <p>
     You have a new message in your Inbox.
    </p>
    <table class="sncore_account_table">
     <tr>
      <td style="text-align: left; vertical-align: top;" class="sncore_table_tr_td">
       <div class="sncore_message_subject">
        <% Response.Write(Renderer.Render(this.AccountMessage.Subject)); %>
       </div>
       <div class="sncore_description">
        from 
        <a href="AccountView.aspx?id=<% Response.Write(this.AccountMessage.SenderAccountId); %>">
         <% Response.Write(Renderer.Render(this.AccountMessage.SenderAccountName)); %>
        </a>
        on
        <% Response.Write(Renderer.Render(this.AccountMessage.Sent.ToString("d"))); %>        
       </div>
       <div style="margin: 10px 0px 10px 0px;">
        <% Response.Write(Renderer.RenderEx(this.AccountMessage.Body)); %>
       </div>
      </td>
     </tr>
    </table>  
    <p>
     <a href="AccountMessageView.aspx?id=<% Response.Write(this.AccountMessage.Id); %>">&#187; Read</a>
     <a href="AccountMessageEdit.aspx?id=<% Response.Write(this.AccountMessage.RecepientAccountId); %>&pid=<% Response.Write(this.AccountMessage.Id); %>">&#187; Reply</a>
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
