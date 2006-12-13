<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemRefererAccountEdit.aspx.cs" Inherits="SystemRefererAccountEdit" Title="System | Referer Account" %>

<%@ Register TagPrefix="SnCore" tagname="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Referer Account
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemRefererAccountsManage.aspx"
  runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_inner_table">
  <tr>
   <td class="sncore_form_label">
    referer host:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputRefererHost" runat="server" />
    <asp:RequiredFieldValidator ID="inputRefererHostRequired" runat="server" ControlToValidate="inputRefererHost"
     CssClass="sncore_form_validator" ErrorMessage="referer host is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    account id:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputAccount" runat="server" />
    <asp:RequiredFieldValidator ID="inputAccountRequired" runat="server" ControlToValidate="inputAccount"
     CssClass="sncore_form_validator" ErrorMessage="account required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true" CssClass="sncore_form_button"
     OnClick="save_Click" />
   </td>
  </tr>
 </table>
</asp:Content>
