<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SearchDiscussionPosts.aspx.cs" Inherits="SearchDiscussionPosts" Title="Search Discussion Posts" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SearchDiscussionPosts" Src="SearchDiscussionPostsControl.ascx" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Search Discussion Posts
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    search:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
    <asp:RequiredFieldValidator ID="inputSearchRequired" runat="server" ControlToValidate="inputSearch"
     CssClass="sncore_form_validator" ErrorMessage="search string is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="search" runat="server" Text="Search!" CausesValidation="true" CssClass="sncore_form_button"
     OnClick="search_Click" />
   </td>
  </tr>
 </table>
 <asp:UpdatePanel runat="server" id="searchPanel" UpdateMode="Always" RenderMode="Inline">
  <ContentTemplate>
   <SnCore:SearchDiscussionPosts id="searchDiscussionPosts" runat="server" />  
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>