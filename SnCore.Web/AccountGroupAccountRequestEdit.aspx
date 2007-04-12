<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountGroupAccountRequestEdit.aspx.cs" Inherits="AccountGroupAccountRequestEdit"
 Title="Group | Join" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Join a Group
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_account_table">
  <tr>
   <td valign="top" class="sncore_table_tr_td" style="text-align: center;">
    <asp:Image ID="imageAccountGroup" ImageUrl="AccountGroupPictureThumbnail.aspx" runat="server" />
    <div>
     <asp:HyperLink ID="linkAccountGroup" runat="server" />
    </div>
   </td>
   <td>
    <table>
     <tr>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputMessage" runat="server" TextMode="MultiLine"
        Rows="10" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="manageSave" runat="server" Text="Send" CausesValidation="true" CssClass="sncore_form_button"
        OnClick="save_Click" />
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
