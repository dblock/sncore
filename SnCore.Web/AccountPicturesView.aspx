<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPicturesView.aspx.cs" Inherits="AccountPicturesView" Title="Account | Pictures" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Account Pictures
 </div>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Always" RenderMode="Inline">
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
       <%# GetCommentCount((int)Eval("CommentCount")) %>
      </a>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>
