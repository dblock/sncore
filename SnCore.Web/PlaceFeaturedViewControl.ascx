<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlaceFeaturedViewControl.ascx.cs"
 Inherits="PlaceFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel ID="panelFeatured" runat="server">
 <link rel="alternate" type="application/rss+xml" title="Rss" href="PlacesRss.aspx" />
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="*" valign="top" class="sncore_featured_tr_td">
    <div class="sncore_h2">
     <a href="PlaceView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
      Featured Place
      <img src="images/site/right.gif" border="0" />
     </a>
    </div>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <div class="sncore_link">
      <a href="PlacesView.aspx">&#187; all places</a> 
      <a href="PlaceEdit.aspx">&#187; suggest a place</a> 
     </div>
     <div class="sncore_link">
      <a href="FeaturedPlacesView.aspx">&#187; previously featured</a>
      <a href="PlacesRss.aspx">&#187; rss</a> </span>
    </asp:Panel>
   </td>
   <td width="150px" align="center" valign="top">
    <a href="PlaceView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
     <img border="0" src="PlacePictureThumbnail.aspx?id=<% Response.Write(base.Place.PictureId); %>" />
    </a>
    <div class="sncore_description">
     <a href="PlaceView.aspx?id=<% Response.Write(base.Place.Id); %>">
      <% Response.Write(base.Render(base.Place.Name)); %>
     </a>
    </div>
    <div class="sncore_description">
     <% Response.Write(base.Render(base.Place.City)); %>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
