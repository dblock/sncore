<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="SystemAccountPropertyEdit.aspx.cs"
 Inherits="SystemAccountPropertyEdit" Title="Account Property" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkAccountEvent" Text="AccountEvent"
   runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkItem" Text="Type" runat="server" />
 </div>
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Account Property
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemAccountPropertiesManage.aspx"
     runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
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
       type:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputTypeName" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       default value:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputDefaultValue" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputPublish" Text="Publish" Checked="true"
        runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="save_Click" />
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
