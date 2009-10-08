<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlacesNewViewControl.ascx.cs"
 Inherits="PlacesNewViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="PlacesRss.aspx" 
 ButtonVisible="false" Title="New Places" />
<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
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
   <div class="sncore_createnew">
    <div class="sncore_link">
     <a href="PlaceEdit.aspx">&#187; add a new place</a>
     <a href="PlacesView.aspx">&#187; all</a>
     <a href="FeaturedPlacesView.aspx">&#187; featured</a>
     <a href="PlacesRss.aspx">&#187; rss</a>
    </div>
   </div>
  </td>
 </tr>
</table>
<asp:DataList CssClass="sncore_half_table" HorizontalAlign="Center" runat="server"
 ID="Places" Width="95%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
 ItemStyle-CssClass="sncore_table_tr_td">
 <ItemTemplate>
  <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
   <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
   <div class="sncore_link_description">
    <%# base.Render(Eval("Name")) %>
   </div>
  </a>
  <div class="sncore_description">
   <%# base.Render(Eval("City"))%>
  </div>
 </ItemTemplate>
</asp:DataList>
