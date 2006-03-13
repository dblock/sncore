<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountCreateOpenId.aspx.cs"
 Inherits="AccountCreateOpenId" Title="Join" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Join!
 </div>
 <div class="sncore_h2sub">
  &#187; <a href="AccountCreate.aspx">join here</a> if you don't have an 
  <a href="http://www.videntity.org" target="_blank">open-id</a>
 </div>
 <asp:ValidationSummary runat="server" ID="inputValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <asp:Panel ID="panelCreateOpenId" runat="server">
  <asp:Panel ID="panelBeta" runat="server">
   <table class="sncore_table">
    <tr>
     <td>
      This site is in <b>beta</b>. We would like to control the size of the user population
      until it reaches critical mass. If you don't have the beta password and wish to
      join, please do
      <asp:LinkButton ID="linkAdministrator" runat="server" Text="e-mail the administrator"
       CausesValidation="false" />
      for an invite.
     </td>
    </tr>
   </table>
   <table class="sncore_table">
    <tr>
     <td class="sncore_form_label" style="color: Red; font-weight: bold;">
      beta password:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputBetaPassword"
       runat="server" />
      <asp:RequiredFieldValidator ID="inputBetaPasswordRequired" runat="server" ControlToValidate="inputBetaPassword"
       CssClass="sncore_form_validator" ErrorMessage="beta password is required" Display="Dynamic" />
     </td>
    </tr>
   </table>
  </asp:Panel>
  <table class="sncore_table">
   <tr>
    <td class="sncore_form_label">
     your name:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
     <asp:RequiredFieldValidator ID="inputNameRequired" runat="server" ControlToValidate="inputName"
      CssClass="sncore_form_validator" ErrorMessage="name is required" Display="Dynamic" />
    </td>
   </tr>
   <tr>
    <td class="sncore_form_label">
     <a href="http://www.videntity.org" target="_blank">your open-id</a>
    </td>
    <td class="sncore_form_value">
     <asp:TextBox CssClass="sncore_form_openid_textbox" ID="inputOpenId" runat="server" />
     <asp:RequiredFieldValidator ID="inputOpenIdRequired" runat="server" ControlToValidate="inputOpenId"
      CssClass="sncore_form_validator" ErrorMessage="open-id is required" Display="Dynamic" />
    </td>
   </tr>
  </table>
  <table class="sncore_table">
   <tr>
    <td class="sncore_form_label">
    </td>
    <td class="sncore_form_value">
     <SnCoreWebControls:Button ID="inputCreateOpenId" runat="server" Text="Join!" CausesValidation="true"
      CssClass="sncore_form_button" OnClick="CreateOpenId_Click" />
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
