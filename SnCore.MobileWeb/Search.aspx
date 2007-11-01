<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="Search.aspx.cs" Inherits="Search" Title="Search" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SearchDiscussionPosts" Src="SearchDiscussionPostsControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchAccounts" Src="SearchAccountsControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchPlaces" Src="SearchPlacesControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchAccountStories" Src="SearchAccountStoriesControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchAccountFeedItems" Src="SearchAccountFeedItemsControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchAccountBlogPosts" Src="SearchAccountBlogPostsControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div>
  <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
  <SnCoreWebControls:Button ID="search" runat="server" Text="Search!" CausesValidation="true" CssClass="sncore_form_button"
   OnClick="search_Click" />
 </div>
 <asp:Panel ID="panelNoResults" CssClass="sncore_description" runat="server" Visible="false">
  Sorry, no results
 </asp:Panel>
 <asp:Panel runat="server" ID="panelSearch">
  <ul>
   <li><asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkPlaces" Text="places" runat="server" OnClick="linkAny_Click"/></li>
   <li><asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkAccounts" Text="people" runat="server" OnClick="linkAny_Click" /></li>
   <li><asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkDiscussionPosts" Text="discussion posts" runat="server" OnClick="linkAny_Click"/> </li>
   <!--<li><asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkAccountStories" Text="stories" runat="server" OnClick="linkAny_Click"/> </li>-->
   <li><asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkAccountFeedItems" Text="syndicated blogs" runat="server" OnClick="linkAny_Click"/> </li>
   <!--<li><asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkAccountBlogPosts" Text="member blogs" runat="server" OnClick="linkAny_Click"/> </li>-->
  </ul>
  <asp:MultiView ID="searchView" runat="server">
   <asp:View runat="server" ID="viewNothing">
   </asp:View>
   <asp:View runat="server" ID="viewAccounts">
    <SnCore:SearchAccounts id="searchAccounts" runat="server" />
   </asp:View>
   <asp:View runat="server" ID="viewPlaces">
    <SnCore:SearchPlaces id="searchPlaces" runat="server" />  
   </asp:View>
   <asp:View runat="server" ID="viewDiscussionPosts">
    <SnCore:SearchDiscussionPosts id="searchDiscussionPosts" runat="server" />  
   </asp:View>
   <!--
   <asp:View runat="server" ID="viewAccountStories">
    <SnCore:SearchAccountStories id="searchAccountStories" runat="server" />  
   </asp:View>
   -->
   <asp:View runat="server" ID="viewAccountFeedItems">
    <SnCore:SearchAccountFeedItems id="searchAccountFeedItems" runat="server" />  
   </asp:View>
   <!--
   <asp:View runat="server" ID="viewAccountBlogPosts">
    <SnCore:SearchAccountBlogPosts id="searchAccountBlogPosts" runat="server" />  
   </asp:View>
   -->
  </asp:MultiView>
 </asp:Panel>
</asp:Content>
