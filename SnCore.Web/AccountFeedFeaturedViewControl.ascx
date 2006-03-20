<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedFeaturedViewControl.ascx.cs"
 Inherits="AccountFeedFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel runat="server" ID="panelFeatured">
 <table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
  <tr>
   <td>
    <link rel="alternate" type="application/rss+xml" title="Rss" href="AccountFeedsRss.aspx" />
    <div class="sncore_h2">
     <a href='AccountFeedsView.aspx'>
      Featured Feed
      <img src="images/site/right.gif" border="0" />
     </a>
    </div>
   </td>
  </tr>
  <tr>
   <td>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <span class="sncore_link"><a href="AccountFeedsView.aspx">&#187; all</a> <a href="AccountFeedEdit.aspx">
      &#187; syndicate a feed</a> <a href="FeaturedAccountFeedsView.aspx">&#187; previously
       featured</a> <a href="AccountFeedsRss.aspx">&#187; rss</a> </span>
    </asp:Panel>
   </td>
  </tr>
 </table>
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="150px" align="center" valign="top">
    <a href="AccountFeedView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<% Response.Write(base.AccountFeed.AccountPictureId); %>" />
    </a>
   </td>
   <td width="*" valign="top">
    <a class="sncore_AccountFeed_name" href="AccountFeedView.aspx?id=<% Response.Write(base.AccountFeed.Id); %>">
     <% Response.Write(base.Render(base.AccountFeed.Name)); %>
    </a>
    <div class="sncore_description">
     <% Response.Write(GetSummary(base.AccountFeed.Description)); %>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
