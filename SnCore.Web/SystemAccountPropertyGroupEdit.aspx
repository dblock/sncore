<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemAccountPropertyGroupEdit.aspx.cs"
 Inherits="SystemAccountPropertyGroupEdit" Title="Account Property Group" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkAccount" Text="Account" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkItem" Text="Property Group" runat="server" />
 </div>
 <div class="sncore_h2">
  Property Group
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemAccountPropertyGroupsManage.aspx"
  runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
    name:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
    <asp:RequiredFieldValidator ID="inputNameRequired" runat="server" ControlToValidate="inputName"
     CssClass="sncore_form_validator" ErrorMessage="name is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    description:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputDescription" runat="server"
     TextMode="MultiLine" Rows="3" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true"
     CssClass="sncore_form_button" OnClick="save_Click" />
   </td>
  </tr>
 </table>
 <asp:Panel ID="panelProperties" runat="server">
  <div class="sncore_h2">
   Properties
  </div>
  <asp:HyperLink ID="linkNewProperty" Text="&#187; Create New" CssClass="sncore_createnew"
   NavigateUrl="SystemAccountPropertyEdit.aspx" runat="server" />
  <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridProperties" PageSize="15"
   AllowPaging="true" OnItemCommand="gridProperties_ItemCommand" AutoGenerateColumns="false"
   CssClass="sncore_account_table">
   <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
    PrevPageText="Prev" HorizontalAlign="Center" />
   <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
   <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
   <Columns>
    <asp:BoundColumn DataField="Id" Visible="false" />
    <asp:TemplateColumn>
     <itemtemplate>
  <img src="images/Item.gif" />
 </itemtemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="Left">
     <itemtemplate>
   <%# base.Render(Eval("Name")) %>
   <div class="sncore_description">
    <%# base.Render(Eval("Description")) %> (<%# base.Render(Eval("Type").ToString()) %>)
   </div>
  </itemtemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn>
     <itemtemplate>
   <a href='SystemAccountPropertyEdit.aspx?id=<%# Eval("Id") %>&pid=<%# Eval("AccountPropertyGroupId") %>'>
    Edit
   </a>
  </itemtemplate>
    </asp:TemplateColumn>
    <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
   </Columns>
  </SnCoreWebControls:PagedGrid>
 </asp:Panel>
</asp:Content>
