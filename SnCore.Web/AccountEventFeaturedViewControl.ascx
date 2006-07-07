<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountEventFeaturedViewControl.ascx.cs"
 Inherits="AccountEventFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel runat="server" ID="panelFeatured">
 <link rel="alternate" type="application/rss+xml" title="Rss" href="AccountEventsRss.aspx" />
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="*" valign="top" class="sncore_featured_tr_td">
    <div class="sncore_h2">
    <a href="AccountEventView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
      Featured Event
      <img src="images/site/right.gif" border="0" />
     </a>
    </div>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <div class="sncore_link">
      <a href="AccountEventsView.aspx">&#187; all</a>
      <a href="FeaturedAccountEventsView.aspx">&#187; previously featured</a>
      <a href="AccountEventsRss.aspx">&#187; rss</a> 
     </div>
     <div class="sncore_link">     
      <a href="AccountEventEdit.aspx">&#187; submit an event</a>
     </div>
    </asp:Panel>
   </td>
   <td width="150px" align="center" valign="top">
    <a href="AccountEventView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
     <img border="0" src="AccountEventPictureThumbnail.aspx?id=<% Response.Write(base.AccountEvent.PictureId); %>" />
    </a>
    <div class="sncore_description">
     <a href="AccountEventView.aspx?id=<% Response.Write(base.AccountEvent.Id); %>">
      <% Response.Write(base.Render(base.AccountEvent.Name)); %>
     </a>
    </div>
    <div class="sncore_description">
     at 
     <a href='PlaceView.aspx?id=<% Response.Write(base.AccountEvent.PlaceId); %>'>
      <% Response.Write(base.AccountEvent.PlaceName); %>
     </a>
    </div>
    <div class="sncore_description">        
     <% Response.Write(base.AccountEvent.PlaceCity); %>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
