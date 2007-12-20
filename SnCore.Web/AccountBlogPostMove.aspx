<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountBlogPostMove.aspx.cs"
 Inherits="AccountBlogPostMove" Title="Move Discussion Post" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Move Post to a Blog
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    destination:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList ID="listBlogs" CssClass="sncore_form_dropdown" DataTextField="Name"
     DataValueField="Id" runat="server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="moveToBlog" runat="server" Text="Move"
     CausesValidation="true" CssClass="sncore_form_button" OnClick="moveToBlog_Click" />
   </td>
  </tr>
 </table>
 <div class="sncore_h2">
  Move Post to a Discussion
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    destination:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList ID="listDiscussions" CssClass="sncore_form_dropdown" DataTextField="Name"
     DataValueField="Id" runat="server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="moveToDiscussion" runat="server" Text="Move"
     CausesValidation="true" CssClass="sncore_form_button" OnClick="moveToDiscussion_Click" />
   </td>
  </tr>
 </table>
</asp:Content>
