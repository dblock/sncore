<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedItemsNewViewControl.ascx.cs"
 Inherits="AccountFeedItemsNewViewControl" %>
<link rel="alternate" type="application/rss+xml" title="Rss" href="AccountFeedsRss.aspx" />
<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
   <div class="sncore_h2">
    <a href='AccountFeedItemsView.aspx'>
      New Blog Posts
     <img src="images/site/right.gif" border="0" />
    </a>
   </div>
  </td>
 </tr>
 <tr>
  <td>
   <div class="sncore_createnew">
    <div class="sncore_link">
     <a href="AccountFeedItemsView.aspx">&#187; all</a>
     <a href="AccountFeedWizard.aspx">&#187; syndicate a blog</a>
     <a href="AccountFeedItemsRss.aspx">&#187; rss</a>
    </div>
   </div>
  </td>
 </tr>
</table>
<asp:DataGrid CellPadding="4" ShowHeader="false" runat="server" ID="FeedsView"
 AutoGenerateColumns="false" CssClass="sncore_half_table">
 <ItemStyle CssClass="sncore_table_tr_td" />
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:TemplateColumn>
   <ItemTemplate>
    <div class="sncore_title">
     <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
      <%# base.Render(GetValue(Eval("Title"), "Untitled"))%>
     </a>
     <span style="font-size: smaller;">
      <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>&#187; <%# GetComments((int) Eval("CommentCount"))%></a>
     </span>
    </div>
    <!--
    <div style="font-size: smaller;">
     by 
     <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
      <%# base.Render(Eval("AccountName"))%>
     </a>
     in
     <a href='AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>'>
      <%# base.Render(Eval("AccountFeedName")) %>
     </a>
     on
     <%# base.Adjust(Eval("Created")).ToString("d") %>
    </div>
    -->
    <div class="sncore_description">
     <%# base.GetDescription((string) Eval("Description"))%>
    </div>
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
