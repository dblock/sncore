<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedItemsFeaturedViewControl.ascx.cs"
 Inherits="AccountFeedItemsFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="FeaturedAccountFeedItemsRss.aspx" 
 Title="Featured Blog Posts" ButtonVisible="false" />
<div id="divTitle" class="sncore_h2" runat="server">
 <a href='AccountFeedItemsView.aspx'>
  Featured Blog Posts
  <img src="images/site/right.gif" border="0" />
 </a>
</div>
<asp:Panel ID="panelLinks" runat="server" CssClass="sncore_createnew">
 <div id="divLinks" class="sncore_link" runat="server">
  <span id="spanLinkViewBlog" runat="server">
   <a href='AccountFeedItemsView.aspx'>
    &#187; read all
   </a>
   <a href='AccountFeedsView.aspx'>
    &#187; blog directory
   </a>
   <a href='FeaturedAccountFeedItemsRss.aspx'>
    &#187; rss
   </a>
  </span>
 </div>
</asp:Panel>
<table class="sncore_half_table">
 <tr>
  <td class="sncore_table_tr_td">
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
    RepeatRows="6" ShowHeader="false" AllowCustomPaging="true">
    <ItemTemplate>
     <table cellpadding="0" cellspacing="0" width="100%">
      <tr>
       <td valign="top">
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
          <%# base.GetImage(GetAccountFeedItem((int)Eval("DataRowId")).Description, GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedLinkUrl)%>
         </a>
       </td>
       <td valign="top">
        <div class="sncore_title">
         <a href='AccountFeedItemView.aspx?id=<%# Eval("DataRowId") %>'>
          <%# base.GetTitle(GetAccountFeedItem((int)Eval("DataRowId")).Title) %>
         </a>
        </div>
        <div class="sncore_link">
         <a target="_blank" href='<%# base.Render(GetAccountFeedItem((int)Eval("DataRowId")).Link) %>'>
          &#187; x-posted
         </a>
         in
         <a href="AccountView.aspx?id=<%# GetAccountFeedItem((int)Eval("DataRowId")).AccountId %>">
          <%# base.Render(GetAccountFeedItem((int)Eval("DataRowId")).AccountName)%>
         </a>'s
         <a href='AccountFeedView.aspx?id=<%# GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedId %>'>
          <%# base.Render(GetValue(GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedName, "Untitled"))%>
         </a>
        </div>
        <div style="margin-top: 10px;">
         <%# base.GetDescription(GetAccountFeedItem((int)Eval("DataRowId")).Description) %>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </td>
 </tr>
</table>
