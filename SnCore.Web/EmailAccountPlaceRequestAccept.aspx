<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountPlaceRequestAccept.aspx.cs"
 Inherits="EmailAccountPlaceRequestAccept" Title="Place ownership request accepted" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Place Ownership Request Accepted
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.AccountPlaceRequest.AccountName)); %></b>,
    </p>
    <p>
     Your ownership request for
     <a href="AccountView.aspx?id=<% Response.Write(this.AccountPlaceRequest.PlaceId); %>">
      <% Response.Write(Renderer.Render(this.AccountPlaceRequest.PlaceName)); %>
     </a>
     has been accepted.
    </p>
    <asp:Panel ID="panelMessage" runat="server">
     <div style="margin: 10px; padding: 10px; border: solid 1px silver; font-style: italic;">
      <% Response.Write(Renderer.RenderEx(Request.QueryString["message"])); %>
     </div>
    </asp:Panel>
    <div>
     <a href="PlaceView.aspx?id=<% Response.Write(this.AccountPlaceRequest.PlaceId); %>">
      &#187; View <% Response.Write(this.AccountPlaceRequest.PlaceName); %>
     </a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
