<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionPostsNewViewControl.ascx.cs"
 Inherits="DiscussionPostsNewViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
   <div class="sncore_h2">
    <a href='DiscussionsView.aspx'>
      New Forum Posts
     <img src="images/site/right.gif" border="0" />
    </a>
   </div>
  </td>
 </tr>
 <tr>
  <td>
   <asp:Panel CssClass="sncore_createnew" ID="panelLinks" runat="server">
    <span class="sncore_link">
     <a href="DiscussionThreadsView.aspx">&#187; new posts</a>
     <a href="DiscussionsView.aspx">&#187; all forums</a>
     <a href="DiscussionThreadsRss.aspx">&#187; rss</a>
    </span>
   </asp:Panel>
  </td>
 </tr>
</table>
<asp:DataGrid CellPadding="4" ShowHeader="false" runat="server" ID="discussionView"
 AutoGenerateColumns="false" CssClass="sncore_half_table">
 <ItemStyle CssClass="sncore_table_tr_td" />
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:TemplateColumn>
   <ItemTemplate>
    <table cellpadding="0" cellspacing="0" width="100%">
     <tr>
      <td width="*">
       <link rel="alternate" type="application/rss+xml" title="Rss" href="DiscussionRss.aspx?id=<%# Eval("DiscussionId") %>" />
       <div class="sncore_title">
        <a href='DiscussionView.aspx?id=<%# Eval("DiscussionId") %>'>
         <%# base.Render(Eval("DiscussionName"))%> 
         <span class="sncore_link">
          <a href='DiscussionView.aspx?id=<%# Eval("DiscussionId") %>'>&#187; read</a>
          <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# 
           Renderer.UrlEncode(Request.Url.PathAndQuery) %>"><%# base.SessionManager.IsLoggedIn ? "&#187; post new" : ""%></a>
          <a href='DiscussionRss.aspx?id=<%# Eval("DiscussionId") %>'>
           &#187; rss</a>
         </span>
        </a>
       </div>
       <div class="sncore_description">
        <a href='DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&id=<%# Eval("DiscussionThreadId") %>&ReturnUrl=<%# SnCore.Tools.Web.Renderer.UrlEncode(Request.Url.PathAndQuery) %>'>
         <%# base.Render(Eval("Subject"))%>
        </a>
       </div>
       <!--
       <div style="font-size: smaller;">
        by 
        <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
         <%# base.Render(Eval("AccountName"))%>
        </a>
        on
        <%# base.Adjust(Eval("Created")).ToString("d") %>
       </div>
       -->
      </td>
     </tr>
    </table>
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
