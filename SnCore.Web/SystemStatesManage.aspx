<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemStatesManage.aspx.cs" Inherits="SystemStatesManage" Title="System | States" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     States
    </div>
    <asp:HyperLink ID="HyperLink1" Text="Create New" CssClass="sncore_createnew" NavigateUrl="SystemStateEdit.aspx"
     runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
     runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
     AllowPaging="True" PageSize="20">
     <PagerStyle CssClass="sncore_account_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
     <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:BoundColumn DataField="Name" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
     <img src="images/Item.gif" />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="State">
       <itemtemplate>
     <%# base.Render(Eval("Name")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Country">
       <itemtemplate>
     <%# base.Render(Eval("Country")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
     <a href='SystemStateEdit.aspx?id=<%# Eval("Id") %>'>Edit</a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete">
      </asp:ButtonColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
