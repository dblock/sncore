<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountsControl.ascx.cs"
 Inherits="SearchAccountsControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelAccountsResults" runat="server">
 <div class="sncore_h2">
  People
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
  <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridResults"
   AllowCustomPaging="true" RepeatColumns="1" RepeatRows="7" RepeatDirection="Horizontal"
   CssClass="sncore_table" ShowHeader="false">
   <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
    prevpagetext="Prev" />
   <ItemTemplate>
    <a href="AccountView.aspx?id=<%# Eval("Id") %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
    </a>
    <div>
     <a href="AccountView.aspx?id=<%# Eval("Id") %>">
      <%# Renderer.Render(Eval("Name")) %>
     </a>
    </div>
    <div class="sncore_description">
     <%# Renderer.Render(Eval("City")) %>
     <%# Renderer.Render(Eval("State")) %>
    </div>
    <div class="sncore_description">
     <%# Renderer.Render(Eval("Country")) %>
    </div>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
</asp:Panel>
