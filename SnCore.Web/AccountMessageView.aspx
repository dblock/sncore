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
   <td style="text-align: left; vertical-align: top;" class="sncore_table_tr_td">
    <div class="sncore_message_subject">
     <asp:Label ID="messageSubject" runat="server" />
    </div>
    <div class="sncore_description">
     <asp:Label ID="labelMessageFrom" runat="server" Text="from" /> <asp:HyperLink ID="messageFrom" runat="server" />
     <asp:Label ID="labelMessageTo" runat="server" Text="to" /> <asp:HyperLink ID="messageTo" runat="server" />
     on <asp:Label ID="messageSent" runat="server" />        
    </div>
    <div class="sncore_description">
     <asp:HyperLink id="linkReply" runat="server" text="&#187; reply" />
     <asp:HyperLink id="linkMove" runat="server" text="&#187; move" />
     <asp:LinkButton id="linkDelete" OnClick="linkDelete_Click" runat="server" text="&#187; delete" 
      OnClientClick="return confirm('Are you sure you want to do this?')" />
    </div>       
    <div style="margin: 10px 0px 10px 0px;">
     <asp:Label ID="messageBody" runat="server" />
    </div>
   </td>
   <td align="center" style="width: 120px;" class="sncore_table_tr_td">
    <a runat="server" id="messageSenderLink">
     <asp:Image Width="100px" runat="server" ID="messageSenderImage" />
     <asp:Label ID="messageSenderName" runat="server" />
    </a>
   </td>
  </tr>
 </table>               
</asp:Content>
