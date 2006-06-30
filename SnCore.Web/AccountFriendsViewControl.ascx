<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFriendsViewControl.ascx.cs"
 Inherits="AccountFriendsViewControl" %>
<%@Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Friends</div>
<SnCoreWebControls:PagedList CssClass="sncore_inner_table" runat="server" ID="friendsList" Width="0px"
 ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="sncore_table_tr_td"
 RepeatColumns="4" RepeatRows="1">
 <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next" PrevPageText="Prev"
		HorizontalAlign="Center" />
 <ItemTemplate>
  <a href="AccountView.aspx?id=<%# Eval("FriendId") %>">
   <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("FriendPictureId") %>" />
   <div class="sncore_link_description">
    <%# base.Render(Eval("FriendName")) %>
   </div>
  </a>
 </ItemTemplate>
</SnCoreWebControls:PagedList>
