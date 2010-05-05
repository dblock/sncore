<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountLoginControl.ascx.cs"
 Inherits="AccountLoginControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Log-In!
</div>
<div class="sncore_h2sub">
 Problems logging in?
 <asp:LinkButton ID="linkAdministrator" runat="server" Text="&#187; E-Mail" CausesValidation="false" />
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
  <td class="sncore_form_label">
   -- or --
  </td>
  <td class="sncore_form_value">
  </td>
 </tr>
 <tr>
  <td class="sncore_form_label">
  </td>
  <td class="sncore_form_value">
    <asp:Panel ID="panelFacebookLogin" runat="server">
       <script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script>
       <script type="text/javascript">
        var facebookAPIKey = "<% Response.Write(SessionManager.GetCachedConfiguration("Facebook.APIKey", "")); %>";
        FB.init(facebookAPIKey, "AccountLogin.aspx");
       </script>
       <fb:login-button onlogin='window.location="AccountLogin.aspx?facebook.login=1";' />
    </asp:Panel>
    <asp:Label ID="facebookLoginDisabled" runat="server" CssClass="sncore_notice_warning" Visible="false"
        Text="Facebook.APIKey has not been set, Facebook login disabled." />
  </td>
 </tr>
 <tr>
  <td class="sncore_form_label">
   -- or --
  </td>
  <td class="sncore_form_value">
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
   <div>
    <a class="sncore_link" href="AccountResetPassword.aspx">&#187; forgot password?</a>
   </div>
   <div>
    <a class="sncore_link" href="AccountCreate.aspx">not a member? &#187; join!</a>
   </div>
  </td>
  <td class="sncore_form_value">
   <SnCoreWebControls:Button ID="loginLogin" runat="server" Text="Log In" CausesValidation="true"
    CssClass="sncore_form_button" OnClick="loginLogin_Click" />
  </td>
 </tr>
</table>
