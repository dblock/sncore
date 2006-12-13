<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="PlacesManage.aspx.cs" Inherits="PlacesManage" Title="Places" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Places
 </div>
 <asp:HyperLink ID="HyperLink1" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="PlaceEdit.aspx"
  runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
  runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
  AllowPaging="True" PageSize="10">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:BoundColumn DataField="Name" Visible="false" />
   <asp:TemplateColumn>
    <itemtemplate>
     <img src="images/Item.gif" />
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Place">
    <itemtemplate>
     <a href='PlaceView.aspx?id=<%# Eval("Id") %>'>
      <%# base.Render(Eval("Name")) %>
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Location">
    <itemtemplate>
     <%# base.Render(Eval("Neighborhood")) %>,
     <%# base.Render(Eval("City")) %>,
     <%# base.Render(Eval("State")) %>,
     <%# base.Render(Eval("Country")) %>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <itemtemplate>
     <a href='PlaceEdit.aspx?id=<%# Eval("Id") %>'>Edit</a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <itemtemplate>
     <a href='PlacePicturesManage.aspx?id=<%# Eval("Id") %>'>Pictures</a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
