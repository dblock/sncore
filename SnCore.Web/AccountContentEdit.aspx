<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountContentEdit.aspx.cs"
 Inherits="AccountContentEdit" Title="Content" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Content
 </div>
 <div>
  <asp:HyperLink ID="linkPreview" Text="&#187; Preview" CssClass="sncore_cancel" NavigateUrl="AccountContentGroup.aspx"
   runat="server" />
 </div>
 <div>
  <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountContentGroupEdit.aspx"
   runat="server" />
 </div>
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
    tag:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox ID="inputTag" runat="server" CssClass="sncore_form_textbox" />
    <div class="sncore_description">
     content tag
    </div>
   </td>
  </tr>
  <tr>
   <td colspan="2" class="sncore_form_value">
    <FTB:FreeTextBox Width="100%" id="inputText" runat="Server" />
    <div class="sncore_description">
     trusted content groups allow un-encoded html and support bb-tags
    </div>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    timestamp:
   </td>
   <td class="sncore_form_value">
    <SnCore:SelectDate ID="inputTimestamp" runat="server" />
    <div class="sncore_description">
     content is sorted in reverse order by timestamp
    </div>
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td>
    <SnCoreWebControls:Button ID="linkSave" CssClass="sncore_form_button" OnClick="save"
     runat="server" Text="Save" OnClientClick="WebForm_OnSubmit();" />
   </td>
  </tr>
 </table>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
