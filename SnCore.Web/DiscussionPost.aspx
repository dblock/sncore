<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionPost.aspx.cs"
 Inherits="DiscussionPostNew" Title="Discussion Post" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel ID="panelPost" runat="server">
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
  <SnCore:Title ID="titleNewPost" Text="New Post" runat="server">  
   <Template>
    <div class="sncore_title_paragraph">
     To post a message, enter a subject, write a message body and hit <b>Post</b>.
    </div>
    <div>
     To include a link, select some text, click the link button <img src="images/buttons/link.gif" align="absmiddle" />
     and enter the target address.
    </div>
    <div class="sncore_title_paragraph">    
     To upload a picture, click <b>Browse</b>, find the picture on your computer and press <b>Upload</b>.
     Pictures are automatically resized to 640x480 and are posted to <a href="AccountPicturesManage.aspx">your profile</a>.
     You can also simply copy &amp; paste a picture from another website, and you can drag and drop pictures around
     the text.
    </div>
   </Template>
  </SnCore:Title>
  <div class="sncore_h2sub">
   You are posting in <asp:HyperLink ID="discussionLabelLink" runat="server" />
  </div>
  <asp:HyperLink ID="linkCancel" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
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
    <td style="vertical-align: top;" class="sncore_form_label">
     add photos:
    </td>
    <td>
     <WilcoWebControls:MultiFileUpload id="files" runat="server" inputcssclass="sncore_form_upload"
      onfilesposted="files_FilesPosted" />
     <asp:HyperLink ID="addFile" runat="server" CssClass="sncore_form_label" NavigateUrl="#">+</asp:HyperLink>
     <br />
     <SnCoreWebControls:Button id="picturesAdd" cssclass="sncore_form_button" runat="server"
      text="Upload" OnClientClick="WebForm_OnSubmit();" />   
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
 </asp:Panel>
</asp:Content>
