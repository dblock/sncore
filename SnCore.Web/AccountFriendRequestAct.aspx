<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFriendRequestAct.aspx.cs" Inherits="AccountFriendRequestAct" Title="Account | Friend Request" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2sub">
  <a href="Default.aspx">&#187; Home</a>  
  <a href="AccountsView.aspx">&#187; All People</a>  
  <a href="AccountFriendsManage.aspx">&#187; My Friends</a>
  <a href="AccountInvitationsManage.aspx">&#187; Invite a Friend</a>  
 </div>
</asp:Content>
