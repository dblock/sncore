<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="TagWordAccountsView.aspx.cs" Inherits="TagWordAccountsView" Title="Users" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Tags
 </div>
 <asp:Label ID="tagSubtitle" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
  AllowCustomPaging="true" RepeatColumns="4" RepeatRows="4" RepeatDirection="Horizontal"
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
    <%# base.Render(Eval("City")) %>
    <%# base.Render(Eval("State")) %>
   </div>
   <div>
    <%# base.Render(Eval("Country")) %>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
</asp:Content>
