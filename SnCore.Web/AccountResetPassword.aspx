<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountResetPassword.aspx.cs" Inherits="AccountResetPassword" Title="Account | Reset Password" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Reset Password
 </div>
 <asp:ValidationSummary runat="server" ID="resetpasswordValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    e-mail address:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="resetpasswordEmailAddress" runat="server" />
    <asp:RequiredFieldValidator ID="resetpasswordEmailAddressRequired" runat="server"
     ControlToValidate="resetpasswordEmailAddress" CssClass="sncore_form_validator"
     ErrorMessage="e-mail address is required" Display="Dynamic" /><asp:RegularExpressionValidator
      ID="resetpasswordEmailAddressRegexRequired" runat="server" ControlToValidate="resetpasswordEmailAddress"
      ValidationExpression=".*@.*\..*" CssClass="sncore_form_validator" ErrorMessage="e-mail address is invalid"
      Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    birthday:
   </td>
   <td class="sncore_form_value">
    <SnCore:SelectDate ID="resetpasswordBirthday" runat="server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="resetPassword" runat="server" Text="Reset" CausesValidation="true"
     CssClass="sncore_form_button" OnClick="resetPassword_Click" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value" style="text-align: right;">
   </td>
  </tr>
 </table>
</asp:Content>
