<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFriendsView.aspx.cs" Inherits="AccountFriendsView" Title="Friends" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:UpdatePanel UpdateMode="Conditional" ID="panelLinks" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       <asp:Label id="labelName" runat="server" />
      </div>
      <div class="sncore_h2sub">
       <asp:HyperLink ID="linkAccount" runat="server" Text="&#187; Back" />
       <a href="AccountsView.aspx">&#187; All People</a>
       <a href="AccountInvitationsManage.aspx">&#187; Invite a Friend</a>
       <a href="RefererAccountsView.aspx">&#187; Top Traffickers</a>
      </div>
     </td>
     <td>
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <a href='AccountFriendsRss.aspx?id=<% Response.Write(RequestId); %>'>
       <img src="images/rss.gif" border="0" />
      </a>
      <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
       href="AccountFriendsRss.aspx?id=<% Response.Write(RequestId); %>" />
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" AllowCustomPaging="true" RepeatColumns="4" RepeatRows="3" 
    RepeatDirection="Horizontal" CssClass="sncore_table" ShowHeader="false">
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
      last activity:
      <%# base.Adjust(Eval("LastLogin")).ToString("d") %>
     </div>
     <div>
      <%# base.Render(Eval("City")) %>
      <%# base.Render(Eval("State")) %>
     </div>
     <div>
      <%# base.Render(Eval("Country")) %>
     </div>
     <div>
      <a href='AccountPicturesView.aspx?id=<%# Eval("Id") %>'>
       <%# GetNewPictures((int) Eval("NewPictures")) %>
      </a>
     </div>
     <div>
      <a href='AccountStoryView.aspx?id=<%# GetAccountStoryId((TransitAccountStory) Eval("LatestStory")) %>'>
       <%# GetAccountStory((TransitAccountStory)Eval("LatestStory"))%>
      </a>
     </div>
     <div>
      <a href='AccountSurveyView.aspx?aid=<%# Eval("Id") %>&id=<%# GetSurveyId((TransitSurvey) Eval("LatestSurvey")) %>'>
       <%# GetSurvey((TransitSurvey)Eval("LatestSurvey"))%>
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
