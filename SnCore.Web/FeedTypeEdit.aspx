<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="FeedTypeEdit.aspx.cs" Inherits="FeedTypeEdit" Title="Feed Type" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Feed Type
    </div>
    <asp:HyperLink ID="linkBack" Text="Cancel" CssClass="sncore_cancel" NavigateUrl="FeedTypesManage.aspx"
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
       xsl:
      </td>
      <td class="sncore_form_value">
       <asp:FileUpload CssClass="sncore_form_upload" ID="inputXsl" runat="server" />
       <br /><asp:Label ID="labelXsl" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       span (rows x columns):
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList ID="inputSpanRows" runat="server" CssClass="sncore_form_dropdown_small" />
       <asp:DropDownList ID="inputSpanColumns" runat="server" CssClass="sncore_form_dropdown_small" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       span preview (rows x columns):
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList ID="inputSpanRowsPreview" runat="server" CssClass="sncore_form_dropdown_small" />
       <asp:DropDownList ID="inputSpanColumnsPreview" runat="server" CssClass="sncore_form_dropdown_small" />
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
