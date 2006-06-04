<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountEventFeaturedViewControl.ascx.cs"
 Inherits="AccountEventFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel runat="server" ID="panelFeatured">
 <table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
  <tr>
   <td>
    <link rel="alternate" type="application/rss+xml" title="Rss" href="AccountEventsRss.aspx" />
    <div class="sncore_h2">
     <a href='AccountEventsView.aspx'>
      Featured Event
      <img src="images/site/right.gif" border="0" />
     </a>
    </div>
   </td>
  </tr>
  <tr>
   <td>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <span class="sncore_link"><a href="AccountEventsView.aspx">&#187; all</a> <a href="AccountEventEdit.aspx">
      &#187; submit an event</a> <a href="FeaturedAccountEventsView.aspx">&#187; previously
       featured</a> <a href="AccountEventsRss.aspx">&#187; rss</a> </span>
    </asp:Panel>
   </td>
  </tr>
 </table>
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="150px" align="center" valign="top">
    <a href="AccountEventView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
     <img border="0" src="AccountEventPictureThumbnail.aspx?id=<% Response.Write(base.AccountEvent.PictureId); %>" />
    </a>
   </td>
   <td width="*" valign="top">
    <a href="AccountEventView.aspx?id=<% Response.Write(base.AccountEvent.Id); %>">
     <% Response.Write(base.Render(base.AccountEvent.Name)); %>
    </a>
    <div class="sncore_description">
     at 
     <a href='PlaceView.aspx?id=<% Response.Write(base.AccountEvent.PlaceId); %>'>
      <% Response.Write(base.AccountEvent.PlaceName); %>
     </a>
     <% Response.Write(base.AccountEvent.PlaceCity); %>
    </div>
    <div class="sncore_description">
     <% Response.Write(GetSummary(base.AccountEvent.Description)); %>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
