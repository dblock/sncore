<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="PlaceTypesManage.aspx.cs" Inherits="PlaceTypesManage" Title="Places | Types" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Place Types
 </div>
 <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="PlaceTypeEdit.aspx"
  runat="server" />
 <asp:UpdatePanel id="panelManageUpdate" runat="server" UpdateMode="Always">
  <ContentTemplate>
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
       <img src='<%# (bool) Eval("DefaultType") ? "images/account/star.gif" : "images/Item.gif" %>' />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Name">
      <itemtemplate>
       <%# base.Render(Eval("Name")) %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href='PlaceTypeEdit.aspx?id=<%# Eval("Id") %>'>Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
