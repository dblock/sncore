<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailBugCreated.aspx.cs"
 Inherits="EmailBugCreated" Title="Bug Submitted" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  New Bug
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AdminAccount.Name)); %></b>,
    </p>
    <p>
     <a href="BugView.aspx?id=<% Response.Write(this.Bug.Id); %>">
      <% Response.Write(Renderer.Render(this.Bug.Type)); %> #<% Response.Write(Renderer.Render(this.Bug.Id)); %>:
      <% Response.Write(Renderer.Render(this.Bug.Subject)); %>
     </a>
     has been submitted by 
     <a href='AccountView.aspx?id=<% Response.Write(this.Bug.AccountId); %>'>
      <% Response.Write(Renderer.Render(this.Bug.AccountName)); %>
     </a>
     in
     <a href='BugProjectBugsManage.aspx?id=<% Response.Write(this.Bug.ProjectId); %>'>
      <% Response.Write(Renderer.Render(this.Bug.ProjectName)); %>
     </a>
     .     
    </p>
    <p>
     <a href="BugView.aspx?id=<% Response.Write(this.Bug.Id); %>">
      &#187; View <% Response.Write(Renderer.Render(this.Bug.Type)); %>
     </a> 
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
