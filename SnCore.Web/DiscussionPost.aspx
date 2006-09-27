<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionPost.aspx.cs"
 Inherits="DiscussionPostNew" Title="Discussion Post" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink NavigateUrl="DiscussionsView.aspx" CssClass="sncore_navigate_item"
   ID="linkDiscussions" Text="Forums" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkDiscussion" Text="Discussion"
   runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkItem" Text="Post" runat="server" />
 </div>
 <table width="100%">
  <tr>
   <td>
    <asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
   </td>
   <td align="center" class="sncore_description">
    <asp:Label ID="discussionDescription" runat="server" />
   </td>
  </tr>
 </table>
 <asp:Panel ID="panelReplyTo" runat="server" Visible="false">
  <table width="100%">
   <tr>
    <td>
     <div class="sncore_h2">
      In Response To
     </div>
    </td>
   </tr>
  </table>
  <table class="sncore_table">
   <tr>
    <td align="center" valign="top" class="sncore_table_tr_td" style="padding-top: 10px;
     width: 120px;">
     <a runat="server" id="accountlink">
      <asp:Image Width="100px" runat="server" ID="replytoImage" />
      <div class="sncore_link">
       <asp:Label ID="replytoAccount" runat="server" />
      </div>
     </a>
    </td>
    <td style="text-align: left; vertical-align: top;" class="sncore_table_tr_td">
     <div class="sncore_message_subject">
      <asp:Label ID="replytoSubject" runat="server" />
     </div>
     <div class="sncore_description">
      posted by
      <asp:HyperLink ID="replytoSenderName" runat="server" />
      on
      <asp:Label ID="replytoCreated" runat="server" />
     </div>
     <div style="margin: 10px 0px 10px 0px;">
      <asp:Label ID="replyToBody" runat="server" />
     </div>
    </td>
   </tr>
  </table>
 </asp:Panel>
 <table width="100%">
  <tr>
   <td width="200">
    <div class="sncore_h2">
     <a name="edit">Post</a>
    </div>
   </td>
  </tr>
 </table>
 <asp:HyperLink ID="linkCancel" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_table">
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
    body:
   </td>
   <td class="sncore_form_value">
    <FTB:FreeTextBox id="inputBody" runat="Server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="post" runat="server" Text="Post" CausesValidation="true"
     CssClass="sncore_form_button" OnClick="post_Click" OnClientClick="WebForm_OnSubmit();" />
   </td>
  </tr>
 </table>
</asp:Content>
