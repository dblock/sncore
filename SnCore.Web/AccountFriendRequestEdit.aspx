<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFriendRequestEdit.aspx.cs" Inherits="AccountFriendRequestEdit"
 Title="Account | Friend Request" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <SnCore:AccountReminder ID="accountReminder" runat="server" />
    <div class="sncore_h2">
     Friend Request
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td valign="top" class="sncore_table_tr_td" style="text-align: center;">
       <asp:Image ID="imageKeen" ImageUrl="images/AccountThumbnail.gif" runat="server" /><br />
       <asp:HyperLink ID="linkKeen" runat="server" />
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
          <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Send" CausesValidation="true" CssClass="sncore_form_button"
           OnClick="save_Click" />
         </td>
        </tr>
       </table>
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
