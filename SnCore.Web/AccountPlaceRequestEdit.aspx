<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountPlaceRequestEdit.aspx.cs" Inherits="AccountPlaceRequestEdit" Title="Account | Place Request" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <asp:Panel ID="panelRequest" runat="server">
  <div class="sncore_h2">
   Claim Ownership
  </div>
  <div class="sncore_h2sub">
   Do you work here? Is this your business?
   You and your coworkers may claim ownership of the place which optionally gives you rights to publish,
   edit and delete content. 
  </div>
  <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
  <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
   ShowSummary="true" />
  <table class="sncore_account_table">
   <tr>
    <td valign="top" class="sncore_table_tr_td" style="text-align: center;">
     <asp:Image ID="imagePlace" ImageUrl="PlacePictureThumbnail.aspx" runat="server" />
     <div>
      <asp:HyperLink ID="linkPlace" runat="server" />
     </div>
    </td>
    <td>
     <table>
      <tr>
       <td class="sncore_form_value">
        <div style="margin: 4px; font-weight: bold">
         Type of relationship:
        </div>
        <asp:DropDownList DataTextField="Name" DataValueField="Name" CssClass="sncore_form_dropdown"
         ID="inputType" runat="server" />
       </td>
      </tr>
      <tr>
       <td class="sncore_form_value">
        <div style="margin: 4px; font-weight: bold">
         Business justification:
        </div>
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputMessage" runat="server" TextMode="MultiLine"
         Rows="10" />
       </td>
      </tr>
      <tr>
       <td class="sncore_form_value">
        <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Send" CausesValidation="true" CssClass="sncore_form_button"
         OnClick="save_Click" />
       </td>
      </tr>
     </table>
    </td>
   </tr>
  </table>
 </asp:Panel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
