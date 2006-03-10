<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemConfigurationsManage.aspx.cs" Inherits="SystemConfigurationsManage"
 Title="System | Configurations" %>

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
    <asp:HyperLink ID="HyperLink1" Text="Create New" CssClass="sncore_createnew" NavigateUrl="SystemConfigurationEdit.aspx"
     runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand" runat="server"
     ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
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
      <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" HeaderStyle-CssClass="sncore_table_tr_th"
       HeaderText="Name">
       <itemtemplate>
     <%# base.Render(Eval("Name")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" HeaderStyle-CssClass="sncore_table_tr_th"
       HeaderText="Value">
       <itemtemplate>
     <%# base.Render(Eval("Value")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="sncore_table_tr_td"
       HeaderStyle-CssClass="sncore_table_tr_th">
       <itemtemplate>
     <a href="SystemConfigurationEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" ButtonType="LinkButton" ItemStyle-CssClass="sncore_table_tr_td"
       HeaderStyle-CssClass="sncore_table_tr_th" CommandName="Delete" Text="Delete">
      </asp:ButtonColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
