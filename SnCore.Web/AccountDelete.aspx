<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountDelete.aspx.cs" Inherits="AccountDelete" Title="Account | Delete" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkAccount" Text="Delete Account"
   runat="server" />
 </div>
 <asp:Panel CssClass="panel" ID="pnlAccount" runat="server">
  <table cellspacing="0" cellpadding="4" class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top;">
     <a runat="server" id="linkPictures" href="AccountView.aspx">
      <img alt="" border="0" src="images/AccountThumbnail.gif" runat="server" id="accountImage" />
     </a>
    </td>
    <td valign="top" class="sncore_table_tr_td">
     <table class="sncore_inner_table">
      <tr>
       <td class="sncore_table_tr_td">
        <asp:Label runat="server" ID="accountName" CssClass="sncore_account_name" Text="Welcome!" />
        <p>
         This will <b>permanently</b> remove your account. We're sad
         to see you go. This cannot be undone. All your personal data will be deleted. Your
         forum posts and tags will remain anonymous in the system.
        </p>
       </td>
      </tr>
     </table>
     <table class="sncore_inner_table">
      <tr>
       <td class="sncore_form_label" style="font-size: 1em;">
        password:
       </td>
       <td class="sncore_form_value" style="font-size: 1em;">
        <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputPassword"
         runat="server" />
        <asp:RequiredFieldValidator ID="inputPasswordRequired" runat="server" ControlToValidate="inputPassword"
         CssClass="sncore_form_validator" ErrorMessage="password is required" Display="Dynamic" />
       </td>
      </tr>
      <tr>
       <td>
       </td>
       <td class="sncore_form_value">
        <SnCoreWebControls:Button ID="buttonDelete" runat="server" Text="Delete" CausesValidation="true"
         CssClass="sncore_form_button" OnClick="delete_Click" />
       </td>
      </tr>
     </table>
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
