<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedFeaturedViewControl.ascx.cs"
 Inherits="AccountFeedFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<link rel="alternate" type="application/rss+xml" title="Rss" href="AccountFeedsRss.aspx" />
<asp:Panel runat="server" ID="panelFeatured">
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="*" valign="top" class="sncore_featured_tr_td">
    <div class="sncore_h2">
     <a href="AccountFeedView.aspx?id=<% Response.Write(base.AccountFeed.Id); %>">
      Featured Feed
      <img src="images/site/right.gif" border="0" />
     </a>
    </div>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <div class="sncore_link">
      <a href="AccountFeedsView.aspx">&#187; all</a>
      <a href="AccountFeedItemImgsView.aspx">&#187; pictures</a>
      <a href="FeaturedAccountFeedsView.aspx">&#187; previously featured</a>
     </div>
     <div class="sncore_link">     
      <a href="AccountFeedEdit.aspx">&#187; syndicate a feed</a>
      <a href="AccountFeedsRss.aspx">&#187; rss</a> 
     </div>
    </asp:Panel>
   </td>
   <td width="150px" align="center" valign="top">
    <a href="AccountFeedView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<% Response.Write(base.AccountFeed.AccountPictureId); %>" />
    </a>
    <div class="sncore_description">
     <a href="AccountFeedView.aspx?id=<% Response.Write(base.AccountFeed.Id); %>">
      <% Response.Write(base.Render(base.AccountFeed.Name)); %>
     </a>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
