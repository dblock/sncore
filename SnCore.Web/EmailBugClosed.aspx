<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailBugClosed.aspx.cs"
 Inherits="EmailBugClosed" Title="Bug closed" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Bug Closed
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.Bug.AccountName)); %></b>,
    </p>
    <p>
     <a href="BugView.aspx?id=<% Response.Write(this.Bug.Id); %>">
      <% Response.Write(Renderer.Render(this.Bug.Type)); %> #<% Response.Write(Renderer.Render(this.Bug.Id)); %>:
      <% Response.Write(Renderer.Render(this.Bug.Subject)); %>
     </a>
     that you have submitted has been resolved and closed. Your feedback is greatly appreciated.
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
