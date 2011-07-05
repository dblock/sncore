<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountsActiveViewControl.ascx.cs"
 Inherits="AccountsActiveViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<div class="sncore_h2">
 <a href='AccountsView.aspx'>
  Online Foodies
  <img src="images/site/right.gif" border="0" />
 </a>
</div>
<asp:DataList CssClass="sncore_half_table" HorizontalAlign="Center" runat="server"
 ID="accounts" Width="95%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
 ItemStyle-CssClass="sncore_table_tr_td">
 <ItemTemplate>
  <a href="AccountView.aspx?id=<%# Eval("Id") %>">
   <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
   <div class="sncore_link_description">
    <%# base.Render(Eval("Name")) %>
   </div>
  </a>
  <div class="sncore_description" style="color: silver;">
   <%# base.Render(Eval("City"))%>
  </div>
 </ItemTemplate>
</asp:DataList>
