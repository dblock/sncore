<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountCreateInvitation.aspx.cs" Inherits="AccountCreateInvitation" Title="SignUp" %>

<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Join!
 </div>
 <asp:ValidationSummary runat="server" ID="inputValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <asp:Panel ID="panelCreate" runat="server">
  <table class="sncore_table">
   <tr>
    <td class="sncore_form_label">
     e-mail address:
    </td>
    <td class="sncore_form_value">
     <asp:Label ID="inputEmailAddress" runat="server" />
    </td>
   </tr>
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
     re-type password:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputPassword2"
      runat="server" />
     <asp:RequiredFieldValidator ID="inputPasswordRequired2" runat="server" ControlToValidate="inputPassword2"
      CssClass="sncore_form_validator" ErrorMessage="password is required twice" Display="Dynamic" />
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
   <tr>
    <td class="sncore_form_label">
     city:</td>
    <td class="sncore_form_value">
     <asp:TextBox CssClass="sncore_form_textbox" ID="inputCity" runat="server" /></td>
   </tr>
   <tr>
    <td class="sncore_form_label">
     country and state:</td>
    <td class="sncore_form_value">
     <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
      CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name"
      DataValueField="Name" runat="server" />
     <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" DataTextField="Name"
      DataValueField="Name" runat="server" />
    </td>
   </tr>
   <tr>
    <td>
    </td>
    <td class="sncore_form_value">
     <SnCoreWebControls:Button ID="inputLogin" runat="server" Text="Sign-Up!" CausesValidation="true"
      CssClass="sncore_form_button" OnClick="create_Click" />
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
