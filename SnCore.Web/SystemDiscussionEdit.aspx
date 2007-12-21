<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemDiscussionEdit.aspx.cs" Inherits="SystemDiscussionEdit" Title="System | Discussion" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Discussion
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemDiscussionsManage.aspx"
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
    default view:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputDefaultView" DataTextField="Description"
     DataValueField="View" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    number of posts:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList ID="inputDefaultViewRows" runat="server" CssClass="sncore_form_dropdown">
     <asp:ListItem Text="None" Value="0" />
     <asp:ListItem Text="1" Value="1" />
     <asp:ListItem Text="2" Value="2" />
     <asp:ListItem Text="3" Value="3" />
     <asp:ListItem Text="4" Value="4" />
     <asp:ListItem Selected="true" Text="5" Value="5" />
     <asp:ListItem Text="6" Value="6" />
     <asp:ListItem Text="7" Value="7" />
     <asp:ListItem Text="8" Value="8" />
     <asp:ListItem Text="9" Value="9" />
     <asp:ListItem Text="10" Value="10" />
     <asp:ListItem Text="15" Value="15" />
     <asp:ListItem Text="20" Value="20" />
     <asp:ListItem Text="25" Value="25" />
    </asp:DropDownList>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <div class="sncore_description">
     number of posts that show in the default views; users can navigate between 
     pages or click on the discussion to see all posts
    </div>
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
</asp:Content>
