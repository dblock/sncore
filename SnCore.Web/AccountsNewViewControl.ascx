<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountsNewViewControl.ascx.cs"
 Inherits="AccountsNewViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<link rel="alternate" type="application/rss+xml" title="Rss" href="AccountsRss.aspx" />
<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
   <div class="sncore_h2">
    <a href='AccountsView.aspx'>
      New People
     <img src="images/site/right.gif" border="0" />
    </a>
   </div>
  </td>
 </tr>
 <tr>
  <td>
   <div class="sncore_createnew">
    <div class="sncore_link">
     <a href="AccountsView.aspx">&#187; all</a>
     <a href="AccountInvitationsManage.aspx">&#187; invite a friend</a>
     <a href="FeaturedAccountsView.aspx">&#187; featured</a>
     <a href="RefererAccountsView.aspx">&#187; top</a>
     <a href="AccountsRss.aspx">&#187; rss</a>
    </div>
   </div>
  </td>
 </tr>
</table>
<asp:DataList CssClass="sncore_half_table" HorizontalAlign="Center" runat="server" ID="accounts" Width="95%"
 ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="sncore_table_tr_td">
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
