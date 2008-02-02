<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountPicturesView.aspx.cs"
 Inherits="AccountPicturesView" Title="Account | Pictures" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" PageSize="10"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false" RepeatColumns="1" RepeatRows="5" RepeatDirection="Vertical">
  <pagerstyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" PageButtonCount="5" />
  <ItemStyle CssClass="sncore_table_tr_td" />
  <ItemTemplate>
   <div>
    <a href="AccountPictureView.aspx?id=<%# Eval("Id") %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("Id") %>"
      alt="<%# Renderer.Render(Eval("Name")) %>" border="0" />
    </a>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("Description")) %>
   </div>
   <div class="sncore_description">
    <a href="AccountPictureView.aspx?id=<%# Eval("Id") %>">
     <%# GetCommentCount((int)Eval("CommentCount")) %>
    </a>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
</asp:Content>
