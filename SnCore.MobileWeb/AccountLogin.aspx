<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountLogin.aspx.cs"
 Inherits="AccountLogin" Title="Login" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    e-mail:
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
    open-id
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="loginOpenId" runat="server" />
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
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button id="loginLogin" runat="server" text="Log In" causesvalidation="true"
     cssclass="sncore_form_button" onclick="loginLogin_Click" />
   </td>
  </tr>
 </table>
</asp:Content>
