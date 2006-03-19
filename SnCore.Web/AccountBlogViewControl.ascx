<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountBlogViewControl.ascx.cs"
 Inherits="AccountBlogViewControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 <% Response.Write(Blog.Name); %>
</div>
<div class="sncore_createnew">
 <span class="sncore_link">
  <a href='AccountBlogView.aspx?id=<% Response.Write(BlogId); %>'>
   &#187; view blog
  </a>
 </span>
</div>
<table class="sncore_half_table">
 <tr>
  <td class="sncore_table_tr_td">
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
    RepeatRows="3" ShowHeader="false" AllowCustomPaging="true">
    <ItemTemplate>
     <div>
      <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
       <%# base.GetTitle(Eval("Title")) %>
      </a>
     </div>
     <div style="font-size: smaller;">
      &#187; 
      by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
       <%# base.Render(Eval("AccountName")) %>
      </a>
      on 
      <%# base.Adjust(Eval("Created")).ToString("d") %>
      <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>&#comments'>
       &#187; read</a>
      <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>&#comments'>
       &#187; <%# GetComments((int) Eval("CommentCount"))%></a>
     </div>
     <div style="font-size: smaller;">
      <%# base.GetDescription(Eval("Body")) %>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </td>
 </tr>
</table>
