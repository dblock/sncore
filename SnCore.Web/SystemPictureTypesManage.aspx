<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemPictureTypesManage.aspx.cs" Inherits="SystemPictureTypesManage"
 Title="Pictures | Types" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkSystem" Text="Pictures" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkSection" Text="Types" runat="server" />
 </div>
 <div class="sncore_h2">
  Picture Types
 </div>
 <asp:HyperLink ID="linkNew" Text="Create New" CssClass="sncore_createnew" NavigateUrl="SystemPictureTypeEdit.aspx"
  runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15" AllowPaging="true"
  OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false" CssClass="sncore_table">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
     <img src="images/Item.gif" />
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" HeaderStyle-CssClass="sncore_table_tr_th"
    HeaderText="Name" ItemStyle-Font-Bold="true">
    <itemtemplate>
     <%# base.Render(Eval("Name")) %>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" HeaderStyle-CssClass="sncore_table_tr_th"
    ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
     <a href='SystemPictureTypeEdit.aspx?id=<%# Eval("Id") %>'>
      Edit</a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" ButtonType="LinkButton" ItemStyle-CssClass="sncore_table_tr_td"
    HeaderStyle-CssClass="sncore_table_tr_th" CommandName="Delete" Text="Delete">
   </asp:ButtonColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
