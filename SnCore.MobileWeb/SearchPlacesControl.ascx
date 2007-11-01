<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchPlacesControl.ascx.cs"
 Inherits="SearchPlacesControl" %>
<%@Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelPlacesResults" runat="server">
 <div class="sncore_h2">
  Places
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridResults"
  AllowCustomPaging="true" CssClass="sncore_table" ShowHeader="false" RepeatColumns="1" 
  RepeatRows="7" RepeatDirection="Horizontal">
  <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" />
  <ItemTemplate>
   <div>
    <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
     <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
    </a>
   </div>
   <div>
    <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
     <%# Renderer.Render(Eval("Name")) %>
    </a>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("Neighborhood")) %>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("City")) %>
    <%# Renderer.Render(Eval("State")) %>
   </div>
   <div>
    <%# Renderer.Render(Eval("Country")) %>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
</asp:Panel>
