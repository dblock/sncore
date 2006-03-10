<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountLoginControl.ascx.cs"
 Inherits="AccountLoginControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Log-In!
</div>
<div class="sncore_h2sub">
 Problems logging in?
 <asp:LinkButton ID="linkAdministrator" runat="server" Text="e-mail" CausesValidation="false" />
</div>
<asp:ValidationSummary runat="server" ID="loginValidationSummary" CssClass="sncore_form_validator"
 ShowSummary="true" />
<table class="sncore_table">
 <tr>
  <td class="sncore_form_label">
   e-mail address:
  </td>
  <td class="sncore_form_value">
   <asp:TextBox CssClass="sncore_form_textbox" ID="loginEmailAddress" runat="server" />
  </td>
 </tr>
 <tr>
  <td class="sncore_form_label">
   password:
  </td>
  <td class="sncore_form_value">
   <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="loginPassword"
    runat="server" />
  </td>
 </tr>
 <tr>
  <td colspan="2" class="sncore_form_label">
   -- or --
  </td>
 </tr>
 <tr>
  <td class="sncore_form_label">
   <a href="http://www.videntity.org" target="_blank">open-id</a>
  </td>
  <td class="sncore_form_value">
   <asp:TextBox CssClass="sncore_form_openid_textbox" ID="loginOpenId" runat="server" />
  </td>
 </tr>
 <tr>
  <td class="sncore_form_label">
  </td>
  <td class="sncore_form_value">
   <asp:CheckBox CssClass="sncore_form_checkbox" ID="loginRememberMe" runat="server"
    Text="remember me" />
  </td>
 </tr>
 <tr>
  <td>
   <a class="sncore_link" href="AccountResetPassword.aspx">forgot password?</a>
   <br />
   <a class="sncore_link" href="AccountCreate.aspx">not a member? join!</a>
  </td>
  <td class="sncore_form_value">
   <SnCoreWebControls:Button ID="loginLogin" runat="server" Text="Log In" CausesValidation="true"
    CssClass="sncore_form_button" OnClick="loginLogin_Click" />
  </td>
 </tr>
</table>
