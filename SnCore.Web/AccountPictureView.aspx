<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPictureView.aspx.cs" Inherits="AccountPictureView" Title="Account | Picture" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccount" Text="Account"
   runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkPictures" Text="Pictures"
   runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkPicture" Text="Picture" runat="server" />
 </div>
 <div class="sncore_h2">
  Account Picture
 </div>
 <asp:HyperLink CssClass="sncore_createnew" ID="linkComments" runat="server" NavigateUrl="#comments" />
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
   <td align="left">
    <img alt="" runat="server" id="inputPicture" src="AccountPictureThumbnail.aspx?id=0" />
   </td>
  </tr>
 </table>
 <a name="comments"></a>
 <SnCore:DiscussionFullView runat="server" ID="discussionComments" PostNewText="&#187; Comment" />
</asp:Content>
