<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemAccountPlaceTypesManage.aspx.cs" Inherits="SystemAccountPlaceTypesManage" Title="Account Places | Types" %>


<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Account Place Types
 </div>
 <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemAccountPlaceTypeEdit.aspx"
  runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
  AllowPaging="true" OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false"
  CssClass="sncore_account_table">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn>
    <itemtemplate>
  <img src="images/Item.gif" />
 </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Name">
    <itemtemplate>
     <%# base.Render(Eval("Name")) %>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Owner">
    <itemtemplate>
     <%# base.Render(Eval("CanWrite")) %>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <itemtemplate>
     <a href='SystemAccountPlaceTypeEdit.aspx?id=<%# Eval("Id") %>'>Edit</a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
