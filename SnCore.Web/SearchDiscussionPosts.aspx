<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SearchDiscussionPosts.aspx.cs" Inherits="SearchDiscussionPosts" Title="Search Discussion Posts" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SearchDiscussionPosts" Src="SearchDiscussionPostsControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkDiscussions" Text="Discussions"
   runat="server" NavigateUrl="DiscussionsView.aspx" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkDiscussion" Text="Discussion"
   runat="server" NavigateUrl="DiscussionView.aspx" Visible="false" />
 </div>
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
 <SnCore:SearchDiscussionPosts id="searchDiscussionPosts" runat="server" />  
</asp:Content>