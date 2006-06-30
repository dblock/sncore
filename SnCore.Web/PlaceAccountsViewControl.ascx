<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlaceAccountsViewControl.ascx.cs"
 Inherits="PlaceAccountsViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Contacts
</div>
<SnCoreWebControls:PagedList CssClass="sncore_inner_table" runat="server" ID="accountsList"
 Width="0px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
 ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="6" RepeatRows="1">
 <pagerstyle cssclass="sncore_table_pager" position="Bottom" nextpagetext="Next"
  prevpagetext="Prev" horizontalalign="Center" />
 <ItemTemplate>
  <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
   <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
   <div class="sncore_link_description">
    <%# base.Render(Eval("AccountName")) %>
   </div>
  </a>
 </ItemTemplate>
</SnCoreWebControls:PagedList>
