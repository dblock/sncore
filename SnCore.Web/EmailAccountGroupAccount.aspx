<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountGroupAccount.aspx.cs"
 Inherits="EmailAccountGroupAccount" Title="Welcome" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Dear <% Response.Write(Renderer.Render(this.AccountGroupAccount.AccountName)); %>
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Welcome to 
     <a href='AccountGroupView.aspx?id=<% Response.Write(this.AccountGroup.Id); %>'>
      <% Response.Write(Renderer.Render(this.AccountGroup.Name)); %>
     </a>
    </p>
    <p>
     <% Response.Write(Renderer.Render(this.AccountGroup.Description)); %>
    </p>
    <p>
     <a href='AccountGroupView.aspx?id=<% Response.Write(this.AccountGroup.Id); %>'>
      &#187; <% Response.Write(Renderer.Render(this.AccountGroup.Name)); %>
     </a>
     <a href="AccountGroupsManage.aspx">&#187; All Your Groups</a>
     <a href="AccountGroupsView.aspx">&#187; All Groups</a>
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
