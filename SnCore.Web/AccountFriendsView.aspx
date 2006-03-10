<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFriendsView.aspx.cs" Inherits="AccountFriendsView" Title="Friends" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Friends
    </div>
   </td>
   <td align="right" valign="middle">
    <a href="AccountFriendsRss.aspx?id=<% Response.Write(AccountId); %>">
     <img border="0" alt="Rss" src="images/rss.gif" /></a>
    <link rel="alternate" type="application/rss+xml" title="Rss" href="AccountFriendsRss.aspx?id=<% Response.Write(AccountId); %>" />
   </td>
  </tr>
  <tr>
   <td colspan="2" class="sncore_h2sub">
    &#187; Make new friends in <asp:HyperLink runat="server" ID="linkNewFriends" Text="your city" /> ! 
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="10"
  AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table" ShowHeader="false"
  AllowCustomPaging="true">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
    <itemtemplate>
     <a href="AccountView.aspx?id=<%# Eval("Id") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <div class="sncore_account_name">
      <a href="AccountView.aspx?id=<%# Eval("Id") %>">
       <%# base.Render(Eval("Name")) %>
      </a>
     </div>
     <div style="color: silver">
      Last activity: <%# base.Adjust(Eval("LastLogin")).ToString() %>
      <br />
      <%# base.Render(Eval("City")) %>
      <%# base.Render(Eval("State")) %>
      <%# base.Render(Eval("Country")) %>
     </div>
     <br />
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
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
