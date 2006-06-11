<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPlaceFavoritesManage.aspx.cs" Inherits="AccountPlaceFavoritesManage" Title="Favorite Places" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="100%">
    <div class="sncore_h2">
     My Favorite Places
    </div>
    <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
     ID="favoritesList" Width="0px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
     OnItemCommand="favoritesList_Command" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="3"
     RepeatRows="4">
     <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
      prevpagetext="Prev" horizontalalign="Center" />
     <ItemTemplate>
       <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
        <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PlacePictureId") %>" />
        <div style="font-size: smaller;">
         <%# base.Render(Eval("PlaceName")) %>
        </div>
       </a>
       <div style="font-size: smaller;">
        <asp:LinkButton Text="&#187; delete" ID="deleteFavorite" runat="server" OnClientClick="return confirm('Are you sure you want to do this?')"
         CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
       </div>
     </ItemTemplate>
    </SnCoreWebControls:PagedList>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
