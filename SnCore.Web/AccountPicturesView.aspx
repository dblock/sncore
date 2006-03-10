<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPicturesView.aspx.cs" Inherits="AccountPicturesView" Title="Account | Pictures" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccount" Text="Account"
   NavigateUrl="AccountPicturesView.aspx" runat="server" />
  <font class="sncore_navigate_item">Pictures</font>
 </div>
 <div class="sncore_h2">
  Account Pictures</div>
 <asp:DataList RepeatColumns="4" runat="server" ID="listView" CssClass="sncore_table">
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <ItemTemplate>
   <a href="AccountPictureView.aspx?id=<%# Eval("Id").ToString() %>">
    <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("Id").ToString() %>"
     alt="<%# base.Render(Eval("Name")) %>" />
    <br />
    <%# base.Render(Eval("Description")) %>
    <br />
    <%# ((int) Eval("CommentCount") >= 1) ? Eval("CommentCount").ToString() + 
     " comment" + (((int) Eval("CommentCount") == 1) ? "" : "s") : "" %>
   </a>
  </ItemTemplate>
 </asp:DataList>
</asp:Content>
