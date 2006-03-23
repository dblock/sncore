<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="BugProjectsManage.aspx.cs" Inherits="BugProjectsManage" Title="Bugs | Projects" %>

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
     Bug Projects
    </div>
    <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="BugProjectEdit.aspx"
     runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15" AllowPaging="true"
     OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false" CssClass="sncore_account_table">
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
     <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
     <img src="images/Item.gif" />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Name">
       <itemtemplate>
     <a href='BugProjectBugsManage.aspx?id=<%# Eval("Id") %>'><%# base.Render(Eval("Name")) %></a>
     <br /><%# base.Render(Eval("Description")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
     <a href='BugProjectEdit.aspx?id=<%# base.Render(Eval("Id")) %>'>
      Edit</a>
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
