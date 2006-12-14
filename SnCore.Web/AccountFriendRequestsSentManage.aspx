<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountFriendRequestsSentManage.aspx.cs"
 Inherits="AccountFriendRequestsSentManage" Title="Account | Sent Friend Requests" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Notice ID="noticeManage" runat="server" />
 <div class="sncore_h2">
  Sent Friend Requests
 </div>
 <div class="sncore_h2sub">
  <a href="AccountFriendRequestsManage.aspx">&#187; Pending Requests</a>
 </div>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
    ID="listSent" Width="0px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
    OnItemCommand="listSent_ItemCommand" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="3"
    RepeatRows="2" AllowCustomPaging="true">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <a href="AccountView.aspx?id=<%# Eval("KeenId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("KeenPictureId") %>" />
      <div style="font-size: smaller;">
       <%# base.Render(Eval("KeenName")) %>
      </div>
     </a>
     <div class="sncore_description">
      <%# base.Adjust(Eval("Created")).ToString() %>
     </div>
     <div style="font-size: smaller;">
      <asp:LinkButton Text="&#187; Cancel" ID="linkCancel" runat="server" OnClientClick="return confirm('Are you sure you want to cancel this request?')"
       CommandName="Cancel" CommandArgument='<%# Eval("Id") %>' />
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
