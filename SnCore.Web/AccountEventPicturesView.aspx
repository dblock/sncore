<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountEventPicturesView.aspx.cs" Inherits="AccountEventPicturesView" Title="AccountEvent | Pictures" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccountEvent" Text="AccountEvent"
   NavigateUrl="AccountEventPicturesView.aspx" runat="server" />
  <font class="sncore_navigate_item">Pictures</font>
 </div>
 <div class="sncore_h2">
  Event Pictures
 </div>
 <div class="sncore_h2sub">
  <a href='AccountEventPicturesManage.aspx?id=<% Response.Write(base.RequestId); %>'>Upload a Picture</a>
 </div>
 <asp:DataList RepeatColumns="4" runat="server" ID="listView" CssClass="sncore_table">
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <ItemTemplate>
   <a href="AccountEventPictureView.aspx?id=<%# Eval("Id") %>">
    <img border="0" src="AccountEventPictureThumbnail.aspx?id=<%# Eval("Id") %>"
     alt="<%# base.Render(Eval("Name")) %>" />
    <br />
    <%# base.Render(Eval("Description")) %>
   </a>
  </ItemTemplate>
 </asp:DataList>
</asp:Content>