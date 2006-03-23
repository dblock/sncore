<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemPictureEdit.aspx.cs" Inherits="SystemPictureEdit" Title="System | Picture" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkSystem" Text="System" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkSection" Text="Pictures" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkItem" Text="Picture" runat="server" />
 </div>
 <div class="sncore_h2">
  Picture
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemPicturesManage.aspx"
  runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_table">
  <tr>
   <td>
    <asp:Image runat="server" ID="imageThumbnail" />
   </td>
   <td>
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
      <td class="sncore_form_label">
       type:</td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputType" DataTextField="Name"
        DataValueField="Name" runat="server" />
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
   </td>
  </tr>
 </table>
</asp:Content>
