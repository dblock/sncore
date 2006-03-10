<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="PlacePictureView.aspx.cs" Inherits="PlacePictureView" Title="Place | Picture" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Place Picture
 </div>
 <table class="sncore_table">
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
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    <img runat="server" id="inputPicture" src="PlacePictureThumbnail.aspx?id=0" />
   </td>
  </tr>
 </table>
</asp:Content>
