<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="BugResolve.aspx.cs" Inherits="BugResolve" Title="Resolve Bug" %>

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
     Resolve Bug
    </div>
    <asp:HyperLink ID="linkBack" Text="Cancel" CssClass="sncore_cancel" NavigateUrl="BugsManage.aspx"
     runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       resolution:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_textbox" ID="selectResolution" DataTextField="Name"
        DataValueField="Name" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       note:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox TextMode="MultiLine" Rows="10" CssClass="sncore_form_textbox" ID="inputNote"
        runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Resolve" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="save_Click" />
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
