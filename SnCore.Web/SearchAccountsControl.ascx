<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountsControl.ascx.cs"
 Inherits="SearchAccountsControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelAccountsResults" runat="server">
 <div class="sncore_h2">
  People
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
  <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridResults"
   AllowCustomPaging="true" RepeatColumns="4" RepeatRows="3" RepeatDirection="Horizontal"
   CssClass="sncore_table" ShowHeader="false">
   <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
    prevpagetext="Prev" horizontalalign="Center" />
   <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
   <ItemTemplate>
    <a href="AccountView.aspx?id=<%# Eval("Id") %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
    </a>
    <div>
     <a href="AccountView.aspx?id=<%# Eval("Id") %>">
      <%# base.Render(Eval("Name")) %>
     </a>
    </div>
    <div>
     last activity:
     <%# base.Adjust(Eval("LastLogin")).ToString("d") %>
    </div>
    <div>
     <%# base.Render(Eval("City")) %>
     <%# base.Render(Eval("State")) %>
    </div>
    <div>
     <%# base.Render(Eval("Country")) %>
    </div>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
</asp:Panel>
