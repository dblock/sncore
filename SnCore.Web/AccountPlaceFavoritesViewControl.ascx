<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountPlaceFavoritesViewControl.ascx.cs" Inherits="AccountPlaceFavoritesViewControl" %>

<%@Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Favorite Places
</div>
<div class="sncore_h2sub">
 <asp:HyperLink runat="server" ID="linkAll" Text="&#187; All Favorite Places" />
</div>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" ID="placesList"
   ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="sncore_table_tr_td"
   RepeatColumns="4" RepeatRows="1" AllowCustomPaging="true">
   <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next" PrevPageText="Prev"
		  HorizontalAlign="Center" />
   <ItemTemplate>
    <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
     <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PlacePictureId") %>" />
     <div class="sncore_link_description">
      <%# base.Render(Eval("PlaceName")) %>
     </div>
    </a>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>
