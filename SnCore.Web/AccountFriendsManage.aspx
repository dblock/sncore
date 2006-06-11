<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFriendsManage.aspx.cs" Inherits="AccountFriendsManage" Title="Account | Friends" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     My Friends
    </div>
    <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
     ID="friendsList" Width="0px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
     OnItemCommand="friendsList_Command" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="3"
     RepeatRows="4">
     <pagerstyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
      prevpagetext="Prev" horizontalalign="Center" />
     <ItemTemplate>
      <a href="AccountView.aspx?id=<%# Eval("FriendId") %>">
       <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("FriendPictureId") %>" /><br />
       <b>
        <%# base.Render(Eval("FriendName")) %>
       </b>
       <div style="font-size: smaller;">
        <asp:LinkButton Text="&#187; delete" ID="deleteFriend" runat="server" OnClientClick="return confirm('Are you sure you want to do this?')"
         CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
       </div>
      </a>
     </ItemTemplate>
    </SnCoreWebControls:PagedList>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
