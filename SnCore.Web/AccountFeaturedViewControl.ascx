<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeaturedViewControl.ascx.cs"
 Inherits="AccountFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel runat="server" ID="panelFeatured">
 <table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
  <tr>
   <td>
    <link rel="alternate" type="application/rss+xml" title="Rss" href="AccountsRss.aspx" />
    <div class="sncore_h2">
     <a href='AccountsView.aspx'>
      Featured Person
      <img src="images/site/right.gif" border="0" />
     </a>
    </div>
   </td>
  </tr>
  <tr>
   <td>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <span class="sncore_link"><a href="AccountsView.aspx">&#187; all</a> <a href="AccountInvitationsManage.aspx">
      &#187; invite a friend</a> <a href="FeaturedAccountsView.aspx">&#187; previously
       featured</a> <a href="AccountsRss.aspx">&#187; rss</a> </span>
    </asp:Panel>
   </td>
  </tr>
 </table>
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="150px" align="center" valign="top">
    <a href="AccountView.aspx?id=<% Response.Write(base.Feature.DataRowId); %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<% Response.Write(base.Account.PictureId); %>" />
    </a>
   </td>
   <td width="*" valign="top">
    <a href="AccountView.aspx?id=<% Response.Write(base.Account.Id); %>">
     <% Response.Write(base.Render(base.Account.Name)); %>
    </a>
    <div class="sncore_description">
     <% Response.Write(base.Render(base.Account.City)); %>
    </div>
    <div class="sncore_description">
     <% Response.Write(GetSummary(base.GetDescription((int)base.Account.Id))); %>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
