<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlaceFeaturedViewControl.ascx.cs"
 Inherits="PlaceFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel ID="panelFeatured" runat="server">
 <table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
  <tr>
   <td>
    <link rel="alternate" type="application/rss+xml" title="Rss" href="PlacesRss.aspx" />
    <div class="sncore_h2">
     Featured Place
    </div>
   </td>
  </tr>
  <tr>
   <td>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <span class="sncore_link"><a href="PlacesView.aspx">&#187; all</a> <a href="PlaceEdit.aspx">
      &#187; suggest a place</a> <a href="FeaturedPlacesView.aspx">&#187; previously featured</a>
      <a href="PlacesRss.aspx">&#187; rss</a> </span>
    </asp:Panel>
   </td>
  </tr>
 </table>
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="150px" align="center" valign="top">
    <a href="PlaceView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
     <img border="0" src="PlacePictureThumbnail.aspx?id=<% Response.Write(base.Place.PictureId); %>" />
    </a>
   </td>
   <td width="*" valign="top">
    <a class="sncore_place_name" href="PlaceView.aspx?id=<% Response.Write(base.Place.Id); %>">
     <% Response.Write(base.Render(base.Place.Name)); %>
    </a>
    <div class="sncore_description">
     <% Response.Write(base.Render(base.Place.City)); %>
    </div>
    <div class="sncore_description">
     <% Response.Write(GetSummary(base.Place.Description)); %>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
