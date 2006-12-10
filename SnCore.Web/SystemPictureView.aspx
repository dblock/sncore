<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemPictureView.aspx.cs" Inherits="SystemPictureView" Title="System | Picture" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  System Picture
 </div>
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
    name:
   </td>
   <td class="sncore_form_value">
    <asp:Label ID="inputName" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    type:
   </td>
   <td class="sncore_form_value">
    <asp:Label ID="inputType" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    description:
   </td>
   <td class="sncore_form_value">
    <asp:Label ID="inputDescription" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    uploaded:
   </td>
   <td class="sncore_form_value">
    <asp:Label ID="inputCreated" runat="server" />
   </td>
  </tr>
 </table>
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
    <img runat="server" id="inputPicture" src="SystemPictureThumbnail.aspx?id=0" />
   </td>
  </tr>
 </table>
</asp:Content>
