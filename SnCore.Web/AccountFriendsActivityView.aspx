<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFriendsActivityView.aspx.cs" Inherits="AccountFriendsActivityView" Title="Friends Activity" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     <asp:Label id="labelName" runat="server" Text="Friends Activity" />
    </div>
   </td>
  </tr>
 </table>
 <asp:UpdatePanel runat="server" ID="panelFriends" RenderMode="Inline" UpdateMode="Conditional">
  <ContentTemplate> 
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridFriends" 
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
