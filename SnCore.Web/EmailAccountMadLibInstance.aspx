<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountMadLibInstance.aspx.cs"
 Inherits="EmailAccountMadLibInstance" Title="New Mad Lib" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  New Mad Lib
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.Recepient.Name)); %></b>,
    </p>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(this.MadLibInstance.AccountId); %>">
      <% Response.Write(Renderer.Render(this.MadLibInstance.AccountName)); %>
     </a>
     has posted a new Mad Lib
     <a href='<% Response.Write(ReturnUrl); %>'>&#187; Read</a>
    </p>
    <table class="sncore_account_table">
     <tr>
      <td style="text-align: left; vertical-align: top;" class="sncore_table_tr_td">
       <div class="sncore_description">
        by 
        <a href="AccountView.aspx?id=<% Response.Write(this.MadLibInstance.AccountId); %>">
         <% Response.Write(Renderer.Render(this.MadLibInstance.AccountName)); %>
        </a>
        on
        <% Response.Write(Renderer.Render(this.MadLibInstance.Created.ToString("d"))); %>        
       </div>
       <div style="margin: 10px 0px 10px 0px;">
        <% Response.Write(RenderMadLib(RenderEx(this.MadLibInstance.Text))); %>
       </div>
      </td>
     </tr>
    </table>  
   </td>
  </tr>
 </table>
</asp:Content>
