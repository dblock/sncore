<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="DiscussionPost.aspx.cs" Inherits="DiscussionPostNew" Title="Discussion Post" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink NavigateUrl="DiscussionsView.aspx" CssClass="sncore_navigate_item"
   ID="linkDiscussions" Text="Discussions" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkDiscussion" Text="Discussion"
   runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkItem" Text="Post" runat="server" />
 </div>
 <br />
 <asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
 <br />
 <asp:Label CssClass="sncore_h2sub" ID="discussionDescription" runat="server" />
 <br />
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
  <a name="edit">Post</a>
 </div>
 <div class="sncore_h2sub">
  some well formed html and <a href="/docs/html/faq.html#faq_discussions_tags" target="_blank">bbtags</a> allowed
 </div>
 <asp:HyperLink ID="linkCancel" Text="Cancel" CssClass="sncore_cancel" runat="server" />
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
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputBody" TextMode="MultiLine"
     Rows="10" runat="server" />
    <asp:RequiredFieldValidator ID="inputBodyRequired" runat="server" ControlToValidate="inputBody"
     CssClass="sncore_form_validator" ErrorMessage="some content is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    signature:
    <br />
    <a class="sncore_link_small" href="AccountPreferencesManage.aspx">edit</a>
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputSignature" TextMode="MultiLine"
     Rows="3" runat="server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="post" runat="server" Text="Post" CausesValidation="true" CssClass="sncore_form_button"
     OnClick="post_Click" />
   </td>
  </tr>
 </table>
</asp:Content>
