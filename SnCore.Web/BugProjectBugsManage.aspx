<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="BugProjectBugsManage.aspx.cs" Inherits="BugProjectBugsManage" Title="Bugs | ProjectBugs" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkBugs" Text="Bugs" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkProjects" NavigateUrl="BugProjectsManage.aspx"
   Text="Projects" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkSection" Text="Project" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkProjectBugs" Text="Bugs" runat="server" />
 </div>
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Project Bugs
    </div>
    <asp:HyperLink ID="linkNew" Text="Create New" CssClass="sncore_createnew" NavigateUrl="BugEdit.aspx"
     runat="server" />
    <table class="sncore_account_table">
     <tr>
      <tr>
       <td class="sncore_form_label">
       </td>
       <td class="sncore_form_value">
        <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxResolvedBugs" runat="server"
         AutoPostBack="True" OnCheckedChanged="optionsChanged" Text="show resolved and closed bugs"
         Checked="false" />
       </td>
      </tr>
    </table>
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15" AllowPaging="true"
     AutoGenerateColumns="false" HeaderStyle-CssClass="sncore_table_tr_th" CssClass="sncore_account_table"
     HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="sncore_table_tr_td"
     ItemStyle-HorizontalAlign="center" AllowSorting="True">
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn HeaderText="T" SortExpression="Type" ItemStyle-CssClass="sncore_table_tr_td">
       <itemtemplate>
     <img src='images/bugs/type_<%# Eval("Type") %>.gif' alt='<%# Eval("Type") %>' />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="#" SortExpression="Id">
       <itemtemplate>
     <a href='BugView.aspx?id=<%# Eval("Id") %>'>#<%# Eval("Id") %></a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Pri" SortExpression="Priority">
       <itemtemplate>
     <img src='images/bugs/priority_<%# Eval("Priority") %>.gif' alt='<%# Eval("Priority") %>' />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
     <img src='images/bugs/status_<%# Eval("Status") %>.gif' alt='<%# Eval("Status") %>' />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Description" SortExpression="Subject">
       <itemtemplate>
     <a href='BugView.aspx?id=<%# Eval("Id") %>'><%# base.Render(Eval("Subject")) %></a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Reported By" SortExpression="AccountId">
       <itemtemplate>
     <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# base.Render(Eval("AccountName")) %></a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Created" SortExpression="Created">
       <itemtemplate>
     <%# ((DateTime) Eval("Created")).ToString("d") %>
    </itemtemplate>
      </asp:TemplateColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
