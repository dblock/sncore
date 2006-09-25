<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountPlaceRequest.aspx.cs"
 Inherits="EmailAccountPlaceRequest" Title="Property ownership request" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Property Ownership Request
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Hello,
    </p>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(Renderer.Render(this.AccountPlaceRequest.AccountId)); %>">
      <% Response.Write(Renderer.Render(this.AccountPlaceRequest.AccountName)); %>
     </a>
     is claiming ownership of
     <a href="PlaceView.aspx?id=<% Response.Write(this.AccountPlaceRequest.PlaceId); %>">
      <% Response.Write(Renderer.Render(this.AccountPlaceRequest.PlaceName)); %>
     </a>
    </p>
    <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
     <% Response.Write(Renderer.RenderEx(this.AccountPlaceRequest.Message)); %>
    </div>
    <div>
     <a href="SystemAccountPlaceRequestsManage.aspx?id=<% Response.Write(this.AccountPlaceRequest.PlaceId); %>">&#187; All Requests</a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
