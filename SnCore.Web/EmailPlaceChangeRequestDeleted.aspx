<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailPlaceChangeRequestDeleted.aspx.cs"
 Inherits="EmailPlaceChangeRequestDeleted" Title="Place change request" %>

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
     Your change request for
     <a href="PlaceView.aspx?id=<% Response.Write(this.PlaceChangeRequest.PlaceId); %>">
      <% Response.Write(Renderer.Render(this.PlaceChangeRequest.PlaceName)); %>
     </a>
     submitted on
     <% Response.Write(this.PlaceChangeRequest.Created.ToString("d")); %>
     has been processed.
    </p>
    <div>
     <a href="PlaceView.aspx?id=<% Response.Write(this.PlaceChangeRequest.PlaceId); %>">&#187; Review</a>
     <a href="AccountPlaceChangeRequestsManage.aspx">&#187; All Requests</a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
