<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="SystemConfigurationsManage.aspx.cs"
 Inherits="SystemConfigurationsManage" Title="System | Configurations" %>

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
     Configuration Settings
    </div>
    <asp:HyperLink ID="HyperLink1" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemConfigurationEdit.aspx"
     runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
     runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
     <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:BoundColumn DataField="Name" Visible="false" />
      <asp:TemplateColumn>
       <itemtemplate>
        <img src="images/Item.gif" />
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Name">
       <itemtemplate>
        <a href="SystemConfigurationEdit.aspx?id=<%# base.Render(Eval("Id")) %>">
         <%# base.Render(Eval("Name")) %>
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Value" ItemStyle-HorizontalAlign="Left">
       <itemtemplate>
        <%# base.Render(Eval("Value")) %>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="SystemConfigurationEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
