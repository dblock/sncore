<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="PlacePicturesView.aspx.cs" Inherits="PlacePicturesView" Title="Place | Pictures" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkPlace" Text="Place"
   NavigateUrl="PlacePicturesView.aspx" runat="server" />
  <font class="sncore_navigate_item">Pictures</font>
 </div>
 <div class="sncore_h2">
  Place Pictures
 </div>
 <div class="sncore_h2sub">
  <a href='PlacePicturesManage.aspx?id=<% Response.Write(base.RequestId); %>'>Upload a Picture</a>
 </div>
 <asp:DataList RepeatColumns="4" runat="server" ID="listView" CssClass="sncore_table">
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <ItemTemplate>
   <a href="PlacePictureView.aspx?id=<%# Eval("Id") %>">
    <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("Id") %>"
     alt="<%# base.Render(Eval("Name")) %>" />
    <br />
    <%# base.Render(Eval("Description")) %>
   </a>
  </ItemTemplate>
 </asp:DataList>
</asp:Content>
