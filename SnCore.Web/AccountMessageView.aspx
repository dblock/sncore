<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountMessageView.aspx.cs" Inherits="AccountMessageView" Title="Account | Message" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Message
 </div>
 <asp:HyperLink ID="linkCancel" Text="&#187; Back"
  CssClass="sncore_createnew" runat="server" />     
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_message_tr_td">
    <div class="sncore_message">
     <div class="sncore_message_subject">
      <asp:Label ID="messageSubject" runat="server" />
     </div>
     <div class="sncore_person">
      <a runat="server" id="messageSenderLink">
       <asp:Image Width="50px" runat="server" ID="messageSenderImage" />
      </a>
     </div>
     <div class="sncore_header">
      <asp:Label ID="labelMessageFrom" runat="server" Text="from" /> <asp:HyperLink ID="messageFrom" runat="server" />
      <asp:Label ID="labelMessageTo" runat="server" Text="to" /> <asp:HyperLink ID="messageTo" runat="server" />
      on <asp:Label ID="messageSent" runat="server" />        
     </div>
     <div class="sncore_content_account">
      <div class="sncore_message_body">
       <asp:Label ID="messageBody" runat="server" />
      </div>
     </div>
     <div class="sncore_footer">
      <asp:HyperLink id="linkReply" runat="server" text="&#187; reply" />
      <asp:HyperLink id="linkMove" runat="server" text="&#187; move" />
      <asp:HyperLink id="linkFlag" runat="server" text="&#187; spam" />
      <asp:LinkButton id="linkDelete" OnClick="linkDelete_Click" runat="server" text="&#187; delete" 
       OnClientClick="return confirm('Are you sure you want to do this?')" />
     </div>
    </div>
   </td>
  </tr>
 </table>               
</asp:Content>
