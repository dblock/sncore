<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="BugNoteEdit.aspx.cs" Inherits="BugNoteEdit" Title="Bug Note" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkBug" Text="Bugs" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkProjects" NavigateUrl="BugProjectsManage.aspx"
   Text="Projects" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkProject" Text="Project"
   runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkBugId" Text="Bug" runat="server" />
 </div>
 <table class="sncore_account_table">
  <tr>
   <td width="100" class="sncore_table_tr_td">
    <asp:Image ID="imageBugType" runat="server" />
    <asp:Label ID="bugType" runat="server" Text="Type" />
   </td>
   <td width="*" class="sncore_table_tr_td">
    <div class="sncore_h2">
     <asp:Label ID="bugId" runat="server" Text="#Id" />
     <asp:Label ID="bugSubject" runat="server" Text="Subject" />
    </div>
   </td>
  </tr>
 </table>
 <div class="sncore_h2">
  Bug Note
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="BugNotesManage.aspx"
  runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
    note:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox TextMode="MultiLine" Rows="10" CssClass="sncore_form_textbox" ID="inputNote"
     runat="server" />
    <asp:RequiredFieldValidator ID="inputNoteRequired" runat="server" ControlToValidate="inputNote"
     CssClass="sncore_form_validator" ErrorMessage="name is required" Display="Dynamic" />
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
