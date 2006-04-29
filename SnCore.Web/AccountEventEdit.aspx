<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountEventEdit.aspx.cs"
 Inherits="AccountEventEdit" Title="Account | Event" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Schedule" Src="ScheduleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SelectPlace" Src="SelectPlaceControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     My Event
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountEventsManage.aspx"
     runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       event name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
       <asp:RequiredFieldValidator ID="inputNameRequired" runat="server" ControlToValidate="inputName"
        CssClass="sncore_form_validator" ErrorMessage="name is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       event type:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_textbox" ID="selectType" DataTextField="Name"
        DataValueField="Name" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       description:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputDescription" runat="server"
        TextMode="MultiLine" Rows="5" />
      </td>
     </tr>
    </table>
    <SnCore:Schedule runat="server" ID="schedule" />
    <SnCore:SelectPlace runat="server" ID="place" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       contact phone:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputPhone" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       contact e-mail:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmail" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       website:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputWebsite" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       cost to attend:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputCost" Text="free" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       publish to calendar:
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputPublish" Checked="true" runat="server" />
      </td>
     </tr>
    </table>
    <asp:Panel ID="panelReminder" runat="server" visible="false">
     <table class="sncore_account_table">
      <tr>
       <td align="center" class="sncore_notice_warning">
        this event has changed, please don't forget to save it
       </td>
      </tr>
     </table>
    </asp:Panel>
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="save_Click" />
      </td>
     </tr>
    </table>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
