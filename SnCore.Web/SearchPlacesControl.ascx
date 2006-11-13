<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchPlacesControl.ascx.cs"
 Inherits="SearchPlacesControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelPlacesResults" runat="server">
 <div class="sncore_h2">
  Places
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridResults"
  AllowCustomPaging="true" CssClass="sncore_table" ShowHeader="false" RepeatColumns="4" 
  RepeatRows="3" RepeatDirection="Horizontal">
  <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" />
  <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" />
  <ItemTemplate>
   <div>
    <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
     <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
    </a>
   </div>
   <div>
    <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
     <%# base.Render(Eval("Name")) %>
    </a>
   </div>
   <div>
    <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
     &#187; read and review
    </a>
   </div>
   <div class="sncore_description">
    <%# base.Render(Eval("City")) %>
    <%# base.Render(Eval("State")) %>
   </div>
   <div>
    <%# base.Render(Eval("Country")) %>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
</asp:Panel>
