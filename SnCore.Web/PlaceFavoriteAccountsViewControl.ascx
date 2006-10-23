<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlaceFavoriteAccountsViewControl.ascx.cs" Inherits="PlaceFavoriteAccountsViewControl" %>

<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Friends
</div>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" ID="accountsList"
   ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
   ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="4" RepeatRows="1" AllowCustomPaging="true">
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
 </ContentTemplate>
</asp:UpdatePanel>
