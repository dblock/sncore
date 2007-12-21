<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountBlogViewControl.ascx.cs"
 Inherits="AccountBlogViewControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLinkControl" Src="AccountContentGroupLinkControl.ascx" %>
<div id="divTitle" class="sncore_h2" runat="server">
 <a href='AccountBlogView.aspx?id=<% Response.Write(BlogId); %>'>
  <% Response.Write(BlogName); %>
  <img src="images/site/right.gif" border="0" />
 </a>
</div>
<asp:Panel ID="panelLinks" runat="server" CssClass="sncore_createnew">
 <div id="divLinks" class="sncore_link" runat="server">
  <span id="spanLinkViewBlog" runat="server">
   <a href='AccountBlogView.aspx?id=<% Response.Write(BlogId); %>'>
    &#187; read all
   </a>
   <a href='AccountBlogRss.aspx?id=<% Response.Write(BlogId); %>'>
    &#187; rss
   </a>
  </span>
 </div>
</asp:Panel>
<SnCore:RssLink ID="linkRelRss" runat="server" ButtonVisible="false" />
<table class="sncore_half_table">
 <tr>
  <td class="sncore_table_tr_td">
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
    RepeatRows="3" ShowHeader="false" AllowCustomPaging="true">
    <ItemTemplate>
     <table cellpadding="0" cellspacing="0" width="100%">
      <tr>
       <td valign="top">
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
          <%# base.GetImage((string) Eval("Body")) %>
         </a>
       </td>
       <td valign="top">
        <div class="sncore_title">
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
          <%# base.GetTitle((string) Eval("Title")) %>
         </a>
         <span>
          <%# ((bool) Eval("Sticky")) ? "<img src='images/buttons/sticky.gif' valign='absmiddle'>" : "" %>
         </span>
        </div>
        <!--
        <div style="font-size: smaller;">
         by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
          <%# base.Render(Eval("AccountName")) %>
         </a>on
         <%# base.Adjust(Eval("Created")).ToString("d") %>
        </div>
        -->
        <div class="sncore_link">
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>&#187; read</a>
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>&#187;
          <%# GetComments((int) Eval("CommentCount"))%>
         </a>
        </div>
        <div style="margin-top: 10px;">
         <%# base.GetDescription((string) Eval("Body")) %>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </td>
 </tr>
</table>
