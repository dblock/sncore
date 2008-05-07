<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountAuditEntriesManage.aspx.cs" Inherits="AccountAuditEntriesManage" Title="My Activity" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFriendAuditEntriesView" Src="AccountFriendAuditEntriesViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:AccountFriendAuditEntriesView id="broadcastView" Broadcast="true" Max="5" Friends="false" runat="server" Title="My Broadcasts" />
 <SnCore:AccountFriendAuditEntriesView id="friendsView" Max="5" Friends="false" runat="server" Title="My Activity" />
</asp:Content>
