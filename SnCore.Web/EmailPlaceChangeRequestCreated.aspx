<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailPlaceChangeRequestCreated.aspx.cs"
 Inherits="EmailPlaceChangeRequestCreated" Title="Place change request" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Change Request
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Hello,
    </p>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(Renderer.Render(this.PlaceChangeRequest.AccountId)); %>">
      <% Response.Write(Renderer.Render(this.PlaceChangeRequest.AccountName)); %>
     </a>
     has submitted changes to
     <a href="PlaceView.aspx?id=<% Response.Write(this.PlaceChangeRequest.PlaceId); %>">
      <% Response.Write(Renderer.Render(this.PlaceChangeRequest.PlaceName)); %>
     </a>.
    </p>
    <div>
     <a href="PlaceChangeRequestMerge.aspx?id=<% Response.Write(this.PlaceChangeRequest.Id); %>">&#187; Review &amp; Merge</a>
     <a href="PlaceChangeRequestsManage.aspx?id=<% Response.Write(this.PlaceChangeRequest.PlaceId); %>">&#187; All Change Requests</a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
