<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailPlacePicture.aspx.cs"
 Inherits="EmailPlacePicture" Title="New Picture" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  New Picture
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Hello,
    </p>
    <p>
     A new picture has been uploaded to 
     <a href="PlaceView.aspx?id=<% Response.Write(this.Place.Id); %>">
      <% Response.Write(Renderer.Render(this.Place.Name)); %>
     </a>
    </p>
    <p style="margin: 10px;">
     <a href="PlacePictureView.aspx?id=<% Response.Write(this.PlacePicture.Id); %>">
      <img border="0" src="PlacePictureThumbnail.aspx?id=<% Response.Write(this.PlacePicture.Id); %>" />
     </a>
    </p>
    <div>
     <a href="PlacePictureView.aspx?id=<% Response.Write(this.PlacePicture.Id); %>">&#187; <% Response.Write(Renderer.Render(this.PlacePicture.Name)); %></a>
     <a href="PlaceView.aspx?id=<% Response.Write(this.Place.Id); %>">&#187; <% Response.Write(Renderer.Render(this.Place.Name)); %></a>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
