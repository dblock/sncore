<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountResetPassword.aspx.cs"
 Inherits="AccountResetPassword" Title="Account | Reset Password" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:UpdatePanel ID="panelResetUpdate" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel ID="panelReset" runat="server">
    <div class="sncore_h2">
     Reset Password
    </div>
    <div class="sncore_h2sub">
     <asp:LinkButton ID="linkAdministrator" runat="server" Text="&#187; Having Trouble?" CausesValidation="false" />
    </div>
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
       e-mail address:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="resetpasswordEmailAddress" runat="server" />
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
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
