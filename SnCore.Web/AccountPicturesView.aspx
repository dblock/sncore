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
  Account Pictures
 </div>
 <atlas:UpdatePanel runat="server" ID="panelGrid" Mode="Always" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" PageSize="10"
    AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
    ShowHeader="false" RepeatColumns="4" RepeatRows="4" RepeatDirection="Horizontal">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
    <ItemTemplate>
     <div>
      <a href="AccountPictureView.aspx?id=<%# Eval("Id") %>">
       <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("Id").ToString() %>" 
        alt="<%# base.Render(Eval("Name")) %>" />
      </a>
     </div>
     <div class="sncore_link">
      <a href="AccountPictureView.aspx?id=<%# Eval("Id") %>">
       <%# base.Render(Eval("Description")) %>
      </a>
     </div>
     <div class="sncore_link">
      <a href="AccountPictureView.aspx?id=<%# Eval("Id") %>">
       <%# ((int) Eval("CommentCount") >= 1) ? Eval("CommentCount").ToString() + 
        " comment" + (((int) Eval("CommentCount") == 1) ? "" : "s") : "" %>
      </a>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </atlas:UpdatePanel>   
</asp:Content>
