<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionPost.aspx.cs"
 Inherits="DiscussionPostNew" Title="New Discussion Post" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel ID="panelPost" runat="server">
  <div class="sncore_h2sub">
   You are <asp:Label ID="labelPostingReplying" Text="posting" runat="server" /> 
   in <asp:HyperLink ID="discussionLabelLink" runat="server" />
  </div>
  <div class="sncore_spacer"></div>
  <asp:Panel ID="panelReplyTo" runat="server" Visible="false">  
   <div class="sncore_message">
    <div class="sncore_message_subject">
     <asp:Label ID="replytoSubject" runat="server" />
    </div>
    <div class="sncore_header">
     posted 
     by <asp:HyperLink ID="replytoSenderName" runat="server" />
     <asp:Label ID="replytoCreated" runat="server" />
    </div>
    <div class="sncore_content">
     <div class="sncore_message_body">
      <asp:Label ID="replyToBody" runat="server" />
     </div>
    </div>
   </div>
  </asp:Panel>
  <div class="sncore_spacer"></div>
  <div id="rowsubject" runat="server">
   <div class="sncore_form_label">
    subject:
   </div>
   <div class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputSubject" runat="server" />
   </div>
  </div>
  <div class="sncore_form_label">
   body:
  </div>
  <div class="sncore_form_value">
   <asp:TextBox CssClass="sncore_form_textbox" Rows="7" TextMode="MultiLine" runat="server" ID="inputBody" />
  </div>
  <div class="sncore_form_value">
   <asp:CheckBox ID="inputSticky" runat="server" Checked="false" Text="Sticky" 
    CssClass="sncore_form_checkbox" />
  <div>
  <div class="sncore_form_value">
   <SnCoreWebControls:Button ID="post" runat="server" Text="Post" CausesValidation="true"
    CssClass="sncore_form_button" OnClick="post_Click" />
  </div>
 </asp:Panel>
</asp:Content>
