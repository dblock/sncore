<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountManage.aspx.cs"
 Inherits="AccountManage" Title="Account | Manage" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountRedirectEdit" Src="AccountRedirectEditControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register tagprefix="SnCore" tagname="Notice" src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="GroupsView" Src="AccountGroupsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFriendAuditEntriesView" Src="AccountFriendAuditEntriesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_account_table">
  <tr>
   <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 150px;">
    <a runat="server" id="linkPictures" href="AccountView.aspx">
     <img border="0" src="AccountPictureThumbnail.aspx" runat="server" id="accountImage" />
    </a>
    <div class="sncore_link">
     <a href="AccountPicturesManage.aspx">
      &#187; upload a picture
     </a>
    </div>
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     <asp:Label CssClass="sncore_account_name" ID="accountName" runat="server" />
    </div>
    <div style="margin-left: 10px;">
     <div>
      <asp:HyperLink ID="accountFirstDegree" NavigateUrl="AccountFriendsView.aspx" runat="server" />
     </div>
     <div>
      <asp:Label ID="accountSecondDegree" runat="server" />
     </div>
     <div>
      <asp:HyperLink ID="accountAllDegrees" NavigateUrl="AccountsView.aspx" runat="server" />
     </div>
     <div>
      <asp:HyperLink ID="accountDiscussionThreads" NavigateUrl="AccountDiscussionThreadsView.aspx" runat="server" />
     </div>
    </div>
   </td>
  </tr>
 </table>
 <SnCore:Notice ID="noticeVerifiedEmail" runat="server" />
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
 <SnCore:GroupsView runat="server" ID="groupsView" PublicOnly="false" />
 <SnCore:AccountFriendAuditEntriesView id="broadcastView" Broadcast="true" Max="5" runat="server" Title="Friends' Broadcasts" />
 <SnCore:AccountFriendAuditEntriesView id="friendsView" Max="5" runat="server" Title="Friends' Activity" />
</asp:Content>
