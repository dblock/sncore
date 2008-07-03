<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountResetPassword.aspx.cs"
 Inherits="AccountResetPassword" Title="Account | Reset Password" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleResetPassword" Text="Reset Password" runat="server" ExpandedSize="100">  
  <Template>
   <div class="sncore_title_paragraph">
    This page allows you to reset your password. The system will e-mail you a new password. You must enter your
    e-mail address and birthday for security reasons. Note that because we can't send you an e-mail to an unverified e-mail
    address for the same security reasons, you cannot reset passwords for accounts without one.
   </div>
  </Template>
 </SnCore:Title>
 <asp:UpdatePanel ID="panelResetUpdate" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel ID="panelReset" runat="server">
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
