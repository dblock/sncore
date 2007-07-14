<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountEmailVerify.aspx.cs" Inherits="AccountEmailVerify" Title="Account | Verify EMail" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel ID="panelVerify" runat="server">
  <div class="sncore_h2">
   Verify E-Mail
  </div>
  <asp:ValidationSummary runat="server" ID="inputValidationSummary" CssClass="sncore_form_validator"
   ShowSummary="true" />
  <table class="sncore_table">
   <tr>
    <td class="sncore_form_label">
     code:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox CssClass="sncore_form_textbox" ID="inputCode" runat="server" />
     <asp:RequiredFieldValidator ID="inputCodeRequired" runat="server" ControlToValidate="inputCode"
      CssClass="sncore_form_validator" ErrorMessage="confirmation code is required"
      Display="Dynamic" />
    </td>
   </tr>
   <tr>
    <td class="sncore_form_label">
     password:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputPassword"
      runat="server" />
     <asp:RequiredFieldValidator ID="inputPasswordRequired" runat="server" ControlToValidate="inputPassword"
      CssClass="sncore_form_validator" ErrorMessage="password is required" Display="Dynamic" />
    </td>
   </tr>
   <tr>
    <td class="sncore_form_label">
    </td>
    <td class="sncore_form_value">
     <div class="sncore_link">
      <a href="AccountResetPassword.aspx">&#187; forgot password</a>
     </div>
    </td>
   </tr>
   <tr>
    <td>
    </td>
    <td class="sncore_form_value">
     <SnCoreWebControls:Button ID="inputVerify" runat="server" Text="Confirm" CausesValidation="true"
      CssClass="sncore_form_button" OnClick="EmailVerify_Click" />
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
