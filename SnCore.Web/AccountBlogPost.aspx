<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountBlogPost.aspx.cs" Inherits="AccountBlogPost" Title="Blog | Post" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Blog Post
 </div>
 <asp:HyperLink ID="linkBack" Text="Cancel" CssClass="sncore_cancel" runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    title:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputTitle" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    post:
   </td>
    <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputBody" runat="server" TextMode="MultiLine"
     Rows="10" />
    <asp:RequiredFieldValidator ID="inputBodyRequired" runat="server" ControlToValidate="inputBody"
     CssClass="sncore_form_validator" ErrorMessage="message is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Post" CausesValidation="true" CssClass="sncore_form_button"
     OnClick="save_Click" />
   </td>
  </tr>
 </table>
</asp:Content>
