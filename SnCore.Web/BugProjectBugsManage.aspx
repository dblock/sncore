<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="BugProjectBugsManage.aspx.cs"
 Inherits="BugProjectBugsManage" Title="Bugs" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Project Bugs
 </div>
 <asp:UpdatePanel ID="panelCommands" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
   <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="BugEdit.aspx"
    runat="server" />
   <asp:LinkButton ID="linkSearch" OnClick="linkSearch_Click" runat="server" Text="&#187; Search" />
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel ID="panelSearch" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelSearchInternal" runat="server">
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       search:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="search" runat="server" Text="Search" CssClass="sncore_form_button"
        OnClick="search_Click" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelBugs" UpdateMode="Conditional">
  <ContentTemplate>
   <table class="sncore_account_table">
    <tr>
     <tr>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxResolvedBugs" runat="server"
        AutoPostBack="True" OnCheckedChanged="optionsChanged" Text="resolved" Checked="false" />
      </td>
      <td>
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxClosedBugs" runat="server"
        AutoPostBack="True" OnCheckedChanged="optionsChanged" Text="closed" Checked="false" />
      </td>
      <td>
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxOpenedBugs" runat="server"
        AutoPostBack="True" OnCheckedChanged="optionsChanged" Text="open" Checked="true" />
      </td>
     </tr>
   </table>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
    AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_account_table" AllowSorting="True"
    AllowCustomPaging="true">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn HeaderText="T" SortExpression="Type_Id">
      <itemtemplate>
       <img src='images/bugs/type_<%# Eval("Type") %>.gif' alt='<%# Eval("Type") %>' />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="#" SortExpression="Bug_Id">
      <itemtemplate>
       <a href='BugView.aspx?id=<%# Eval("Id") %>'>#<%# Eval("Id") %></a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Pri" SortExpression="Priority_Id">
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
     <asp:TemplateColumn HeaderText="Reported By" SortExpression="Account_Id">
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
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
