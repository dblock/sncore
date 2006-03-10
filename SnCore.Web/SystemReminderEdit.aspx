<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemReminderEdit.aspx.cs" Inherits="SystemReminderEdit" Title="Reminder" %>

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
     Reminder
    </div>
    <asp:HyperLink ID="linkBack" Text="Cancel" CssClass="sncore_cancel" NavigateUrl="SystemRemindersManage.aspx"
     runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       subject:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputSubject" runat="server" />
       <asp:RequiredFieldValidator ID="inputSubjectRequired" runat="server" ControlToValidate="inputSubject"
        CssClass="sncore_form_validator" ErrorMessage="subject is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       body (html, no dear/thanks):
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputBody" TextMode="MultiLine"
        Rows="7" runat="server" />
       <asp:RequiredFieldValidator ID="inputBodyRequired" runat="server" ControlToValidate="inputBody"
        CssClass="sncore_form_validator" ErrorMessage="body is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       delta (hours):
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputDeltaHours" runat="server">
        <asp:ListItem Text="Every 1 Hour" Value="1" />
        <asp:ListItem Text="Every 6 Hours" Value="6" />
        <asp:ListItem Text="Every 12 Hours" Value="12" />
        <asp:ListItem Text="Every 24 Hours" Value="24" />
        <asp:ListItem Text="Every 48 Hours" Value="48" />
        <asp:ListItem Text="Every 72 Hours" Value="72" />
        <asp:ListItem Text="Once a Week" Value="168" Selected="true" />
        <asp:ListItem Text="Twice a Month" Value="336" />
        <asp:ListItem Text="Every 3 Weeks" Value="504" />
        <asp:ListItem Text="Once a Month" Value="672" />
       </asp:DropDownList>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       watch:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputDataObject" OnSelectedIndexChanged="inputDataObject_SelectedIndexChanged"
        DataTextField="Name" DataValueField="Id" AutoPostBack="True" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       field:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputDataObjectField" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputRecurrent" runat="server"
        Text="Recurrent" Checked="True" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputEnabled" runat="server"
        Text="Enabled" Checked="False" />
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
