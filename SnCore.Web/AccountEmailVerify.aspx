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
     confirmation id:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox CssClass="sncore_form_textbox" ID="inputId" runat="server" />
     <asp:RequiredFieldValidator ID="inputIdRequired" runat="server" ControlToValidate="inputId"
      CssClass="sncore_form_validator" ErrorMessage="id is required"
      Display="Dynamic" />
    </td>
   </tr>
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
