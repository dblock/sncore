<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountFlagView.aspx.cs" Inherits="AccountFlagView" Title="Account | Flag" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Reported Abuse
 </div>
 <asp:HyperLink ID="linkCancel" Text="&#187; Back"
  CssClass="sncore_createnew" runat="server" />     
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_message_tr_td">
    <div class="sncore_message">
     <div class="sncore_person">
      <a runat="server" id="flagAccountImageLink">
       <asp:Image Width="50px" runat="server" ID="flagAccountImage" />
      </a>
     </div>
     <div class="sncore_header">
      <asp:HyperLink ID="linkAccount" runat="server" />
      reporting
      <b><asp:Label ID="flagType" runat="server" /></b>
      from
      <asp:HyperLink ID="linkFlaggedAccount" runat="server" />
      <asp:Label ID="flagCreated" runat="server" />        
     </div>
     <div class="sncore_content_account">
      <div class="sncore_message_body">
       <asp:Label ID="flagDescription" runat="server" />
      </div>
     </div>
     <div class="sncore_footer">
      <asp:HyperLink id="linkReply" runat="server" text="&#187; send message" />
      <asp:LinkButton id="linkDelete" OnClick="linkDelete_Click" runat="server" text="&#187; delete" 
       OnClientClick="return confirm('Are you sure you want to do this?')" />
     </div>
    </div>
   </td>
  </tr>
 </table>               
</asp:Content>
