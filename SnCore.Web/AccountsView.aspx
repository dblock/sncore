<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountsView.aspx.cs"
 Inherits="AccountsView" Title="People" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel UpdateMode="Conditional" ID="panelLinks" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titlePeople" Text="People" runat="server">
       <Template>
        <div class="sncore_title_paragraph">
         This page shows all members, most active members first. You can also search people in your city
         or around the world. Click on someone's photo and add them to your friends!
        </div>
        <div class="sncore_title_paragraph">
         Tip: <a href="AccountPicturesManage.aspx">Upload a picture</a> and your profile will appear on this page.
        </div>
       </Template>
      </SnCore:Title>      
      <div class="sncore_h2sub">
       <asp:LinkButton ID="linkAll" OnClick="linkAll_Click" runat="server" Text="&#187; All People" />
       <asp:LinkButton ID="linkLocal" OnClick="linkLocal_Click" runat="server" Text="&#187; All Local People" />
       <asp:LinkButton ID="linkBloggers" OnClick="linkBloggers_Click" runat="server" Text="&#187; All Bloggers" />
       <a href="AccountGroupsView.aspx">&#187; Groups</a>
       <a href="AccountInvitationsManage.aspx">&#187; Invite a Friend</a>
       <a href="RefererAccountsView.aspx">&#187; Top Traffickers</a>
       <asp:LinkButton ID="linkSearch" OnClick="linkSearch_Click" runat="server" Text="&#187; Search" />
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
       <asp:Hyperlink id="linkPermalink" NavigateUrl="AccountsView.aspx" runat="server" Text="&#187; Permalink" />
       <a href="AccountCitiesView.aspx">&#187; Top Cities</a>
      </div>
     </td>
     <td width="200">
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel ID="panelSearch" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelSearchInternal" runat="server">
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
       name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       e-mail address:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmailAddress" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       sort by:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="listboxSelectSortOrder" runat="server">
        <asp:ListItem Selected="True" Text="Last Activity" Value="LastLogin" />
        <asp:ListItem Text="Name" Value="Name" />
        <asp:ListItem Text="Date Joined" Value="Created" />
       </asp:DropDownList>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       order by:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="listboxSelectOrderBy" runat="server"
        AutoPostBack="True">
        <asp:ListItem Selected="True" Text="Descending" Value="false" />
        <asp:ListItem Text="Ascending" Value="true" />
       </asp:DropDownList>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       country and state:
      </td>
      <td class="sncore_form_value">
       <asp:UpdatePanel runat="server" ID="panelCountryState" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown_small" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
          ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
          runat="server" />
         <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" AutoPostBack="true"
          DataTextField="Name" DataValueField="Name" runat="server" /></td>
        </ContentTemplate>
       </asp:UpdatePanel>
     </tr>
     <tr>
      <td class="sncore_form_label">
       city:
      </td>
      <td class="sncore_form_value">
       <asp:UpdatePanel runat="server" ID="panelCity" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
          DataValueField="Name" runat="server" AutoPostBack="true" />
        </ContentTemplate>
       </asp:UpdatePanel>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxPicturesOnly" runat="server"
        Text="show people with pictures only" Checked="true" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxBloggersOnly" runat="server"
        Text="show bloggers only" Checked="false" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="search" runat="server" Text="Search" CssClass="sncore_form_button"
        OnClick="search_Click" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" RenderMode="Inline" UpdateMode="Conditional">
  <ContentTemplate> 
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" OnDataBinding="gridManage_DataBinding"
    AllowCustomPaging="true" RepeatColumns="4" RepeatRows="3" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <a href="AccountView.aspx?id=<%# Eval("Id") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
     </a>
     <div>
      <a href="AccountView.aspx?id=<%# Eval("Id") %>">
       <%# base.Render(Eval("Name")) %>
      </a>
     </div>
     <div>
      last seen:
      <%# SessionManager.ToAdjustedString((DateTime) Eval("LastLogin")) %>
     </div>
     <div>
      <%# base.Render(Eval("City")) %>
      <%# base.Render(Eval("State")) %>
     </div>
     <div>
      <%# base.Render(Eval("Country")) %>
     </div>
     <div>
      <a href='AccountFriendsView.aspx?id=<%# Eval("Id") %>'>
       <%# GetNewFriends((int) Eval("NewFriends")) %>
      </a>
     </div>
     <div>
      <a href='AccountPicturesView.aspx?id=<%# Eval("Id") %>'>
       <%# GetNewPictures((int) Eval("NewPictures")) %>
      </a>
     </div>
     <div>
      <a href='AccountDiscussionThreadsView.aspx?id=<%# Eval("Id") %>'>
       <%# GetNewDiscussionPosts((int) Eval("NewDiscussionPosts")) %>
      </a>
     </div>
     <div>
      <a href='AccountView.aspx?id=<%# Eval("Id") %>'>
       <%# GetNewSyndicatedContent((int) Eval("NewSyndicatedContent")) %>
      </a>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>