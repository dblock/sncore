<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailRefererAccount.aspx.cs"
 Inherits="EmailRefererAccount" Title="Thanks for the link!" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Thanks for the link!
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.RefererAccount.AccountName)); %></b>,
    </p>
    <p>
     Thank you for adding a link to
     <a href='<% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost"))); %>'>
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
     </a>
     on
     <a href='<% Response.Write(Renderer.Render(this.RefererAccount.RefererHostLastRefererUri)); %>'>
      <% Response.Write(Renderer.Render(this.RefererAccount.RefererHostName)); %>
     </a>!
    </p>
    <p>
     We have listed you on <a href="RefererAccountsView.aspx">Top Traffickers</a> and provided a link back.
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
