<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemAccountEventTypesManage.aspx.cs"
 Inherits="SystemAccountEventTypesManage" Title="Account Events | Types" %>


<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Event Types
 </div>
 <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemAccountEventTypeEdit.aspx"
  runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
  AllowPaging="true" OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false"
  CssClass="sncore_account_table">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn>
    <itemtemplate>
  <img src="images/Item.gif" />
 </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
  <%# base.Render(Eval("Name")) %>
 </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <itemtemplate>
  <a href='SystemAccountEventTypeEdit.aspx?id=<%# Eval("Id") %>'>
   Edit
  </a>
 </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
