<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemRefererHostDupsManage.aspx.cs"
 Inherits="SystemRefererHostDupsManage" Title="RefererHostDups" %>

<%@ Register TagPrefix="SnCoreWebControls" namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Referer Host Dups
 </div>
 <asp:HyperLink ID="HyperLink1" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemRefererHostDupEdit.aspx"
  runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
  AllowPaging="true" OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false"
  CssClass="sncore_account_table">
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn HeaderText="Host" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <b><%# base.Render(Eval("Host")) %></b>
     <br />=&gt; <%# base.Render(Eval("RefererHost")) %>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
     <a href='SystemRefererHostDupEdit.aspx?id=<%# Eval("Id") %>'>
      Edit</a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" ButtonType="LinkButton" ItemStyle-CssClass="sncore_table_tr_td"
    HeaderStyle-CssClass="sncore_table_tr_th" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
