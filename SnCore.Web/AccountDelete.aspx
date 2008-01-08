<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountDelete.aspx.cs"
 Inherits="AccountDelete" Title="Account | Delete" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel CssClass="panel" ID="pnlAccount" runat="server">
  <table cellspacing="0" cellpadding="4" class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top;">
     <a runat="server" id="linkPictures" href="AccountView.aspx">
      <img alt="" border="0" src="AccountPictureThumbnail.aspx" runat="server" id="accountImage" />
     </a>
    </td>
    <td valign="top" class="sncore_table_tr_td">
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_table_tr_td">
        <asp:Label runat="server" ID="accountName" CssClass="sncore_account_name" Text="Welcome!" />
        <p>
         This will <b>permanently</b> remove your account. We're sad to see you go. This
         cannot be undone. All your personal data will be deleted. Your discussion posts,
         comments, places and tags will remain anonymous in the system.
        </p>
       </td>
      </tr>
     </table>
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_form_label">
       </td>
       <td class="sncore_form_value">
        <asp:CheckBox ID="inputConfirm" runat="server" Text="I understand that this cannot be undone."
         Checked="false" Font-Bold="True" ForeColor="Red" />
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
