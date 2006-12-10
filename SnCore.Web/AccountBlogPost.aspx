<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountBlogPost.aspx.cs"
 Inherits="AccountBlogPostNew" Title="Blog | Post" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Blog Post
 </div>
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td align="center" width="150">
    <a runat="server" id="linkAccount" href="AccountView.aspx">
     <img border="0" src="images/AccountThumbnail.gif" runat="server" id="imageAccount" />
     <div>
      <asp:Label ID="labelAccountName" runat="server" />
     </div>
    </a>
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     <asp:Label ID="labelBlog" runat="server" Text="Blog" />
    </div>
    <div class="sncore_h2sub">
     <asp:Label ID="labelBlogDescription" runat="server" />
    </div>
   </td>
  </tr>
 </table>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    title:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputTitle" runat="server" />
   </td>
  </tr>
  <tr>
   <td style="vertical-align: top;" class="sncore_form_label">
    add photos:
   </td>
   <td>
    <WilcoWebControls:MultiFileUpload id="files" runat="server" inputcssclass="sncore_form_upload"
     onfilesposted="files_FilesPosted" />
    <asp:HyperLink ID="addFile" runat="server" CssClass="sncore_form_label" NavigateUrl="#">+</asp:HyperLink>
    <br />
    <SnCoreWebControls:Button id="picturesAdd" cssclass="sncore_form_button" runat="server"
     text="Upload" OnClientClick="WebForm_OnSubmit();" />   
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    post:
   </td>
   <td class="sncore_form_value">
    <FTB:FreeTextBox ID="inputBody" runat="Server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Post" CausesValidation="true"
     CssClass="sncore_form_button" OnClick="save_Click" OnClientClick="WebForm_OnSubmit();" />
   </td>
  </tr>
 </table>
</asp:Content>
