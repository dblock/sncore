<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlacesNewViewControl.ascx.cs"
 Inherits="PlacesNewViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
   <link rel="alternate" type="application/rss+xml" title="Rss" href="PlacesRss.aspx" />
   <div class="sncore_h2">
    <a href='PlacesView.aspx'>
      New Places
     <img src="images/site/right.gif" border="0" />
    </a>
   </div>
  </td>
 </tr>
 <tr>
  <td>
   <asp:Panel CssClass="sncore_createnew" ID="panelLinks" runat="server">
    <span class="sncore_link">
     <a href="PlacesView.aspx">&#187; all</a>
     <a href="PlaceEdit.aspx">&#187; suggest a place</a>
     <a href="FeaturedPlacesView.aspx">&#187; featured</a>
     <a href="PlacesRss.aspx">&#187; rss</a>
    </span>
   </asp:Panel>
  </td>
 </tr>
</table>
<asp:DataList CssClass="sncore_half_table" HorizontalAlign="Center" runat="server"
 ID="Places" Width="95%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
 ItemStyle-CssClass="sncore_table_tr_td">
 <ItemTemplate>
  <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
   <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
   <div style="font-weight: bold;">
    <%# base.Render(Eval("Name")) %>
   </div>
  </a>
  <div style="color: silver;">
   <%# base.Render(Eval("City"))%>
  </div>
 </ItemTemplate>
</asp:DataList>
