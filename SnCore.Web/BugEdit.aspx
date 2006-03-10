<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="BugEdit.aspx.cs" Inherits="BugEdit" Title="Bug" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkBug" Text="Bugs" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkProjects" NavigateUrl="BugProjectsManage.aspx"
   Text="Projects" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkProject" Text="Project"
   runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkBugId" Text="Bug" runat="server" />
 </div>
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Bug / Feature / Case
    </div>
    <asp:HyperLink ID="linkBack" Text="Cancel" CssClass="sncore_cancel" NavigateUrl="BugProjectBugsManage.aspx"
     runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       type:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_textbox" ID="selectType" DataTextField="Name"
        DataValueField="Name" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       title:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputSubject" runat="server" />
       <asp:RequiredFieldValidator ID="inputSubjectRequired" runat="server" ControlToValidate="inputSubject"
        CssClass="sncore_form_validator" ErrorMessage="title is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       priority:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_textbox" ID="selectPriority" DataTextField="Name"
        DataValueField="Name" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       severity:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_textbox" ID="selectSeverity" DataTextField="Name"
        DataValueField="Name" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       details:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox TextMode="MultiLine" Rows="20" CssClass="sncore_form_textbox" ID="inputDetails"
        runat="server" />
       <asp:RequiredFieldValidator ID="inputDetailsRequired" runat="server" ControlToValidate="inputDetails"
        CssClass="sncore_form_validator" ErrorMessage="details are required" Display="Dynamic" />
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
