<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="PlacePictureEdit.aspx.cs" Inherits="PlacePictureEdit" Title="Place | Picture" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Picture
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="PlacePicturesManage.aspx"
  runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_inner_table">
  <tr>
   <td class="sncore_form_label">
    name:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
    <asp:RequiredFieldValidator ID="inputNameRequired" runat="server" ControlToValidate="inputName"
     CssClass="sncore_form_validator" ErrorMessage="name is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    description:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputDescription" runat="server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true" CssClass="sncore_form_button"
     OnClick="save_Click" />
   </td>
  </tr>
 </table>
 <table class="sncore_inner_table">
  <tr>
   <td colspan="2">
    <asp:Image Width="558" runat="server" ID="imageFull" />
   </td>
  </tr>
 </table>
</asp:Content>
