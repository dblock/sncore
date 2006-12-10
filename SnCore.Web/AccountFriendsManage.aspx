<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountFriendsManage.aspx.cs" Inherits="AccountFriendsManage" Title="Account | Friends" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleFriends" Text="My Friends" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Got friends? <a href="AccountsView.aspx">Find people</a> you want to be friends with and click
    on the <b>add to friends</b> link on their profile to send a friends request.
   </div>
  </Template>
 </SnCore:Title>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
    ID="friendsList" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
    OnItemCommand="friendsList_Command" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="3"
    RepeatRows="3" AllowCustomPaging="true">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <a href="AccountView.aspx?id=<%# Eval("FriendId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("FriendPictureId") %>" />
      <div style="font-size: smaller;">
       <%# base.Render(Eval("FriendName")) %>
      </div>
     </a>
     <div style="font-size: smaller;">
      <asp:LinkButton Text="&#187; Delete" ID="deleteFriend" runat="server" OnClientClick="return confirm('Are you sure you want to do this?')"
       CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
