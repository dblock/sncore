<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemCountriesManage.aspx.cs" Inherits="SystemCountriesManage" Title="System | Countries" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Countries
 </div>
 <asp:HyperLink ID="HyperLink1" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemCountryEdit.aspx"
  runat="server" />
 <asp:UpdatePanel id="panelManageUpdate" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
    runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
    AllowCustomPaging="true" AllowPaging="True" PageSize="20">
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
    <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="Name" Visible="false" />
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
      <itemtemplate>
    <img src="images/Item.gif" />
   </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Country">
      <itemtemplate>
    <%# base.Render(Eval("Name")) %>
   </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
    <a href='SystemCountryEdit.aspx?id=<%# Eval("Id") %>'>Edit</a>
   </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
