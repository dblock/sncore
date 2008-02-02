<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlacesFavoritesView.aspx.cs"
 Inherits="PlacesFavoritesView" Title="Favorite Places" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel runat="server" ID="panelGridFavorites" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <asp:Panel id="panelFavorites" runat="server">
    <SnCore:Title ID="titleFavorites" Text="Favorites" runat="server" ExpandedSize="100">
     <Template>
      <div class="sncore_title_paragraph">
       These are world-wide, all-time favorite places. Click on a picture and then add a place to your own favorites.
       More people add a place to favorites, higher it will be ranked and more chances a place gets to appear
       on this exclusive list.
      </div>
     </Template>
    </SnCore:Title>
      <div class="sncore_h2sub">
       <a href="PlacesView.aspx">&#187; All Places</a>
       <a href="PlaceEdit.aspx">&#187; Suggest a Place</a>
       <a href="AccountPlaceQueueManage.aspx">&#187; My Queue</a>
       <a href="PlaceFriendsQueueView.aspx">&#187; My Friends Queue</a>
      </div>
    <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManageFavorites"
     AllowCustomPaging="true" CssClass="sncore_table"
     ShowHeader="false" RepeatColumns="4" RepeatRows="3" RepeatDirection="Horizontal">
     <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
      prevpagetext="Prev" horizontalalign="Center" />
     <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
     <ItemTemplate>
      <div class="sncore_link">
       <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
        <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
       </a>
      </div>
      <div class="sncore_link">
       <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
        <%# base.Render(Eval("Name")) %>
       </a>
      </div>
      <div class="sncore_link">
       <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
        &#187; read and review
       </a>
      </div>
      <div class="sncore_description">
       <%# base.Render(Eval("Neighborhood")) %>
      </div>
      <div class="sncore_description">
       <%# base.Render(Eval("City")) %>
       <%# base.Render(Eval("State")) %>
      </div>
      <div class="sncore_description">
       <%# base.Render(Eval("Country")) %>
      </div>
     </ItemTemplate>
    </SnCoreWebControls:PagedList>
   </asp:Panel>
  </ContentTemplate>
 </asp:UpdatePanel> 
</asp:Content>
