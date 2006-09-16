<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="TellAFriend.aspx.cs" Inherits="TellAFriend" Title="Untitled Page" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table width="100%">
  <tr>
   <td>
    <div class="sncore_h2">
     Tell a Friend
    </div>
    <div class="sncore_h2sub">
     <asp:HyperLink ID="linkCancel" runat="server" Text="&#187; Cancel" />
    </div>
   </td>
   <td width="*" align="center">
    <div class="sncore_description">
     We'll send your friend a full copy of <asp:HyperLink ID="linkPage" runat="server" Text="this page" Target="_blank" /> along with a note.
    </div>
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    Subject:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox ID="inputSubject" CssClass="sncore_form_textbox" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    Friend's E-Mail(s):
   </td>
   <td class="sncore_form_value">
    <asp:TextBox ID="inputEmailAddress" TextMode="MultiLine" Rows="3" CssClass="sncore_form_textbox" runat="server" />
    <div class="sncore_description">
     each address on a separate line
    </div>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    Note:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox ID="inputNote" CssClass="sncore_form_textbox" TextMode="MultiLine" Rows="3" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="send" runat="server" Text="Send"
     CssClass="sncore_form_button" OnClick="send_Click" />    
   </td>
  </tr>
 </table>
</asp:Content>

