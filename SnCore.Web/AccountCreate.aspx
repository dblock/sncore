<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountCreate.aspx.cs"
 Inherits="AccountCreate" Title="Join" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Join!
 </div>
 <asp:ValidationSummary runat="server" ID="inputValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
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
 <asp:Panel ID="panelCreate" runat="server">
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
     birthday:
    </td>
    <td class="sncore_form_value">
     <SnCore:SelectDate ID="inputBirthday" runat="server" />
    </td>
   </tr>
  </table>
  <table class="sncore_table">
   <tr>
    <td class="sncore_form_label">
     e-mail address:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmailAddress" runat="server" />
    </td>
   </tr>
   <tr>
    <td class="sncore_form_label">
     password:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputPassword"
      runat="server" />
    </td>
   </tr>
   <tr>
    <td class="sncore_form_label">
     re-type password:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputPassword2"
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
     <asp:TextBox CssClass="sncore_form_openid_textbox" ID="inputOpenId" runat="server" />
    </td>
   </tr>
  </table>
  <div style="font-size: smaller; margin-left: 20px;">
   -- optional --
  </div>
  <table class="sncore_table">
   <tr>
    <td class="sncore_form_label">
     city:</td>
    <td class="sncore_form_value">
     <asp:TextBox CssClass="sncore_form_textbox" ID="inputCity" runat="server" /></td>
   </tr>
   <tr>
    <td class="sncore_form_label">
     country and state:
    </td>
    <td class="sncore_form_value">
     <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
      CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name" DataValueField="Name"
      runat="server" />
     <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" DataTextField="Name"
      DataValueField="Name" runat="server" />
    </td>
   </tr>
  </table>
  <table class="sncore_table">
   <tr>
    <td class="sncore_form_label">
    </td>
    <td class="sncore_form_value">
     <SnCoreWebControls:Button ID="inputLogin" runat="server" Text="Sign-Up!" CausesValidation="true"
      CssClass="sncore_form_button" OnClick="create_Click" />
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
