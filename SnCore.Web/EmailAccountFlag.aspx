<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountFlag.aspx.cs"
 Inherits="EmailAccountFlag" Title="Flag" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Reported Abuse
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <table class="sncore_account_table">
     <tr>
      <td style="text-align: left; vertical-align: top;" class="sncore_table_tr_td">
       <div class="sncore_description">
        <a href="AccountView.aspx?id=<% Response.Write(this.AccountFlag.AccountId); %>">
         <% Response.Write(Renderer.Render(this.AccountFlag.AccountName)); %>
        </a>
        reported
        <b><% Response.Write(Renderer.Render(this.AccountFlag.AccountFlagType)); %></b>
        from
        <a href="AccountView.aspx?id=<% Response.Write(this.AccountFlag.FlaggedAccountId); %>">
         <% Response.Write(Renderer.Render(this.AccountFlag.FlaggedAccountName)); %>
        </a>
        on
        <% Response.Write(Renderer.Render(this.AccountFlag.Created.ToString("d"))); %>
       </div>
       <div style="margin: 10px 0px 10px 0px;">
        <% Response.Write(Renderer.RenderEx(this.AccountFlag.Description)); %>
       </div>
      </td>
     </tr>
    </table>  
    <p>
     <a href="AccountFlagView.aspx?id=<% Response.Write(this.AccountFlag.Id); %>">&#187; View</a>
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
