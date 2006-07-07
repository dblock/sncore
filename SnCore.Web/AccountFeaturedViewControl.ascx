<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeaturedViewControl.ascx.cs"
 Inherits="AccountFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel runat="server" ID="panelFeatured">
 <link rel="alternate" type="application/rss+xml" title="Rss" href="AccountsRss.aspx" />
 <table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="*" valign="top" class="sncore_featured_tr_td">
    <div class="sncore_h2">
    <a href="AccountView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
      Featured Person
      <img src="images/site/right.gif" border="0" />
     </a>
    </div>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <div class="sncore_link">
      <a href="AccountsView.aspx">&#187; all</a> 
      <a href="AccountInvitationsManage.aspx">&#187; invite a friend</a>
      <a href="FeaturedAccountsView.aspx">&#187; previously featured</a>
     </div>
     <div class="sncore_link">     
      <a href="RefererAccountsView.aspx">&#187; top traffickers</a>
      <a href="TagWordsView.aspx">&#187; tags</a> </span>
      <a href="AccountsRss.aspx">&#187; rss</a> </span>
    </asp:Panel>
   </td>
   <td width="150px" align="center" valign="top">
    <a href="AccountView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<% Response.Write(base.Account.PictureId); %>" />
    </a>
    <div class="sncore_description">
     <a href="AccountView.aspx?id=<% Response.Write(base.Account.Id); %>">
      <% Response.Write(base.Render(base.Account.Name)); %>
     </a>
    </div>
    <div class="sncore_description">
     <% Response.Write(base.Render(base.Account.City)); %>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
