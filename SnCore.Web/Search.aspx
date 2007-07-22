<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="Search.aspx.cs" Inherits="Search" Title="Search" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SearchDiscussionPosts" Src="SearchDiscussionPostsControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchAccounts" Src="SearchAccountsControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchPlaces" Src="SearchPlacesControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchAccountStories" Src="SearchAccountStoriesControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchAccountFeedItems" Src="SearchAccountFeedItemsControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchAccountBlogPosts" Src="SearchAccountBlogPostsControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleSearch" Text="Search" runat="server" ExpandedSize="100">
  <Template>
   <div class="sncore_title_paragraph">
    Search places, people, discussion posts, blogs and more. The smart full text search engine will match
    exact names first, so it will return a person or a place if you know it. The search will then be expanded
    to the entire site, going deep into public discussions and blogs. There's no special syntax, just type what
    you are looking for.
   </div>
  </Template>
 </SnCore:Title>      
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
     OnClick="search_Click" SubmitControl="inputSearch" />
   </td>
  </tr>
 </table>
 <asp:Panel ID="panelNoResults" CssClass="sncore_note" runat="server" Visible="false">
  Sorry, no results
 </asp:Panel>
 <asp:UpdatePanel runat="server" ID="panelSearch" UpdateMode="Always" RenderMode="Inline">
  <ContentTemplate>
   <table class="sncore_table">
    <tr>
     <td colspan="2" class="sncore_table_tr_td">
      <asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkPlaces" Text="places" runat="server" OnClick="linkAny_Click"/> 
      | <asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkAccounts" Text="people" runat="server" OnClick="linkAny_Click" />
      | <asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkDiscussionPosts" Text="discussion posts" runat="server" OnClick="linkAny_Click"/> 
      | <asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkAccountStories" Text="stories" runat="server" OnClick="linkAny_Click"/> 
      | <asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkAccountFeedItems" Text="syndicated blogs" runat="server" OnClick="linkAny_Click"/> 
      | <asp:LinkButton CausesValidation="false" CssClass="sncore_link" ID="linkAccountBlogPosts" Text="member blogs" runat="server" OnClick="linkAny_Click"/> 
     </td>
    </tr>
   </table>
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
    <asp:View runat="server" ID="viewAccountStories">
     <SnCore:SearchAccountStories id="searchAccountStories" runat="server" />  
    </asp:View>
    <asp:View runat="server" ID="viewAccountFeedItems">
     <SnCore:SearchAccountFeedItems id="searchAccountFeedItems" runat="server" />  
    </asp:View>
    <asp:View runat="server" ID="viewAccountBlogPosts">
     <SnCore:SearchAccountBlogPosts id="searchAccountBlogPosts" runat="server" />  
    </asp:View>
   </asp:MultiView>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
