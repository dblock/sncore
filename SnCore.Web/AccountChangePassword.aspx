<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountChangePassword.aspx.cs" Inherits="AccountChangePassword" Title="Account | Change Password" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" style="width: 150px;">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" style="width: 100%;">
    <div class="sncore_h2">
     Change Password
    </div>
    <asp:Panel runat="server" ID="panelChangePassword">
     <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
      ShowSummary="true" />
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_form_label">
        old password:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputOldPassword" runat="server"
         TextMode="Password" />
       </td>
      </tr>
      <tr>
       <td class="sncore_form_label">
        new password:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputNewPassword" runat="server"
         TextMode="Password" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="inputNewPassword"
         CssClass="sncore_form_validator" ErrorMessage="new password is required" Display="Dynamic" />
       </td>
      </tr>
      <tr>
       <td class="sncore_form_label">
        retype new password:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputNewPassword2" runat="server"
         TextMode="Password" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="inputNewPassword2"
         CssClass="sncore_form_validator" ErrorMessage="new password is required twice"
         Display="Dynamic" />
       </td>
      </tr>
      <tr>
       <td>
       </td>
       <td class="sncore_form_value">
        <SnCoreWebControls:Button ID="manageAccountChangePassword" runat="server" Text="Change" CausesValidation="true"
         CssClass="sncore_form_button" OnClick="changePassword_Click" />
       </td>
      </tr>
     </table>
    </asp:Panel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
