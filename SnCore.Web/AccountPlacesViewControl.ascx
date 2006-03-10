<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountPlacesViewControl.ascx.cs" Inherits="AccountPlacesViewControl" %>

<%@Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Places</div>
<SnCoreWebControls:PagedList CssClass="sncore_inner_table" runat="server" ID="placesList" Width="0px"
 ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="sncore_table_tr_td"
 RepeatColumns="6" RepeatRows="1">
 <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next" PrevPageText="Prev"
		HorizontalAlign="Center" />
 <ItemTemplate>
  <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
   <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PlacePictureId") %>" /><br />
   <b>
    <%# base.Render(Eval("PlaceName")) %>
   </b></a>
 </ItemTemplate>
</SnCoreWebControls:PagedList>
