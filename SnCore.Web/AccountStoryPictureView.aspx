<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountStoryPictureView.aspx.cs" Inherits="AccountStoryPictureView" Title="AccountStoryPicture | Picture" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccount" Text="Account"
   runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccountStory" Text="Story"
   runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="labelPicture" Text="Picture" runat="server" />
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    <img runat="server" id="inputPicture" src="AccountStoryPicture.aspx?id=0" />
   </td>
  </tr>
 </table>
</asp:Content>
