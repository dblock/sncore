<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountGroupAccountRequestAccept.aspx.cs"
 Inherits="EmailAccountGroupAccountRequestAccept" Title="Your request was accepted" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Request Accepted
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountGroupAccountRequest.AccountName)); %></b>,
    </p>
    <p>
     Welcome to 
     <a href="AccountGroupView.aspx?id=<% Response.Write(this.AccountGroupAccountRequest.AccountGroupId); %>">
      <% Response.Write(Renderer.Render(this.AccountGroupAccountRequest.AccountGroupName)); %>
     </a>
     !
    </p>
    <asp:Panel ID="panelMessage" runat="server">
     <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
      <% Response.Write(Renderer.RenderEx(Request.QueryString["message"])); %>
     </div>
    </asp:Panel>
    <div>
     <a href="AccountGroupView.aspx?id=<% Response.Write(this.AccountGroupAccountRequest.AccountGroupId); %>">
      &#187; View <% Response.Write(this.AccountGroupAccountRequest.AccountGroupName); %>
     </a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
