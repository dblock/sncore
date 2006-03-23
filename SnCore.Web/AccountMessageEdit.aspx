<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountMessageEdit.aspx.cs" Inherits="AccountMessageEdit" Title="Account | Message" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:Panel ID="panelReplyTo" runat="server" Visible="false">
  <div class="sncore_h2">
   In Response To
  </div>
  <table class="sncore_table">
   <tr>
    <td align="center" style="width: 120px;" class="sncore_table_tr_td">
     <a runat="server" id="accountlink">
      <asp:Image Width="100px" runat="server" ID="replytoImage" />
      <asp:Label ID="replytoAccount" runat="server" />
     </a>
    </td>
    <td style="text-align: left; vertical-align: top;" class="sncore_table_tr_td">
     <b>subject:</b>
     <asp:Label ID="replytoSubject" runat="server" />
     <br />
     <b>posted:</b>
     <asp:Label ID="replytoCreated" runat="server" />
     <br />
     <br />
     <asp:Label ID="replyToBody" runat="server" />
    </td>
   </tr>
  </table>
 </asp:Panel>
 <div class="sncore_h2">
  <a name="edit">Send Message</a>
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td" style="text-align: center;">
    <asp:Image ID="imageAccountTo" ImageUrl="images/AccountThumbnail.gif" runat="server" /><br />
    <asp:HyperLink ID="linkAccountTo" runat="server" />
   </td>
   <td>
    <table>
     <tr>
      <td class="sncore_form_label">
       subject:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputSubject" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       message:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputBody" runat="server" TextMode="MultiLine"
        Rows="10" />
       <asp:RequiredFieldValidator ID="inputBodyRequired" runat="server" ControlToValidate="inputBody"
        CssClass="sncore_form_validator" ErrorMessage="message is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Send" CausesValidation="true" CssClass="sncore_form_button"
        OnClick="save_Click" />
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
