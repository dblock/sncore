<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountEventEdit.aspx.cs"
 Inherits="AccountEventEdit" Title="Account | Event" %>

<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Schedule" Src="ScheduleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SelectPlace" Src="SelectPlaceControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleEvent" Text="New Event" runat="server" ExpandedSize="200">
  <Template>
   <div class="sncore_title_paragraph">
    Post an Event! It's <b>FREE</b>!
   </div>
   <div class="sncore_title_paragraph">
    First, enter event details, such as the event name and the event description and choose an event type
    most appropriate for your event.
   </div>
   <div class="sncore_title_paragraph">
    You can schedule <b>one-time events</b> and <b>recurrent events</b>. One-time events have a start 
    and an end date and time. Recurrent events can occur <b>daily</b>, <b>weekly</b>, <b>monthly</b> 
    or <b>yearly</b>.       
   </div>
   <div class="sncore_title_paragraph">
    Events must occur at a particular location. You can either <b>lookup an existing location</b> or
    <b>add a new location</b>. When you lookup a location, and there's more than one matching result,
    don't forget to <b>choose</b> it. When you create a new location, don't forge to <b>save</b> it.
   </div>
  </Template>
 </SnCore:Title>      
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountEventsToday.aspx"
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
    <asp:TextBox ID="inputDescription" runat="server" TextMode="MultiLine" Rows="5" CssClass="sncore_form_textbox" />
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
 <asp:UpdatePanel runat="server" ID="panelReminderUpdate" UpdateMode="Conditional">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel ID="panelReminder" runat="server" visible="false">
    <table class="sncore_account_table">
     <tr>
      <td align="center" class="sncore_notice_warning">
       this event has changed, please don't forget to save it
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true"
     CssClass="sncore_form_button" OnClick="save_Click" OnClientClick="WebForm_OnSubmit();" />
   </td>
  </tr>
 </table>
</asp:Content>
