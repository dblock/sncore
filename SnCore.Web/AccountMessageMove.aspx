<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountMessageMove.aspx.cs" Inherits="AccountMessageMove" Title="Account | Message" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     Move Message
    </div>
    <asp:HyperLink ID="linkCancel" Text="&#187; Cancel"
     CssClass="sncore_createnew" runat="server" />     
    <table class="sncore_account_table">
     <tr>
      <td align="center" style="width: 120px;" class="sncore_table_tr_td">
       <a runat="server" id="messageSenderLink">
        <asp:Image Width="100px" runat="server" ID="messageSenderImage" />
        <asp:Label ID="messageSenderName" runat="server" />
       </a>
      </td>
      <td style="text-align: left; vertical-align: top;" class="sncore_table_tr_td">
       <div>
        <b>from:</b>
        <asp:HyperLink ID="messageFrom" runat="server" />
       </div>
       <div>
        <b>to:</b>
        <asp:HyperLink ID="messageTo" runat="server" />
       </div>
       <div>
        <b>subject:</b>
        <asp:Label ID="messageSubject" runat="server" />
       </div>
       <div>
        <b>sent:</b>
        <asp:Label ID="messageSent" runat="server" />
       </div>
       <div style="margin: 10px 0px 10px 0px;">
        <b>move to:</b>
        <asp:DropDownList CssClass="sncore_form_dropdown" ID="listFolders" DataTextField="FullPath" 
         DataValueField="Id" runat="server" AutoPostBack="true" OnSelectedIndexChanged="listFolders_SelectedIndexChanged" />
       </div>
      </td>
     </tr>
    </table>               
   </td>
  </tr>
 </table>
</asp:Content>
