<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountMessageEdit.aspx.cs" Inherits="AccountMessageEdit" Title="Account | Message" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel ID="panelReplyTo" runat="server" Visible="false">
  <div class="sncore_h2">
   In Response To
  </div>
  <table style="margin-left: 10px;">
   <tr>
    <td>
     <div class="sncore_message">
      <div class="sncore_message_subject">
       <asp:Label ID="messageSubject" runat="server" />
      </div>
      <div class="sncore_person">
       <a runat="server" id="accountlink">
        <asp:Image Width="50px" runat="server" ID="replytoImage" />
       </a>
      </div>
      <div class="sncore_header">
       <asp:Label ID="labelMessageFrom" runat="server" Text="from" /> <asp:HyperLink ID="messageFrom" runat="server" />
       <asp:Label ID="labelMessageTo" runat="server" Text="to" /> <asp:HyperLink ID="messageTo" runat="server" />
       on <asp:Label ID="messageSent" runat="server" />        
      </div>
      <div class="sncore_content">
       <div class="sncore_message_body">
        <asp:Label ID="messageBody" runat="server" />
       </div>
      </div>
     </div>
    </td>
   </tr>
  </table>
 </asp:Panel>
 <asp:Panel ID="panelMessage" runat="server">
  <div class="sncore_h2">
   <a name="edit">Send Message</a>
  </div>
  <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
  <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
   ShowSummary="true" />
  <table class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td" style="text-align: center;">
     <a runat="server" ID="linkAccountTo2">
      <asp:Image BorderStyle="None" ID="imageAccountTo" ImageUrl="AccountPictureThumbnail.aspx" runat="server" /><br />
     </a>
     <asp:HyperLink ID="linkAccountTo" runat="server" />
    </td>
    <td>
     <table>
      <tr>
       <td class="sncore_form_value">
        <b>subject:</b>
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputSubject" runat="server" />
       </td>
      </tr>
      <tr>
       <td class="sncore_form_value">
        <FTB:FreeTextBox id="inputBody" runat="Server" />
       </td>
      </tr>
      <tr>
       <td class="sncore_form_value">
        <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Send" CausesValidation="true" CssClass="sncore_form_button"
         OnClick="save_Click" OnClientClick="WebForm_OnSubmit();" />
       </td>
      </tr>
     </table>
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
