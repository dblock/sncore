<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionView.aspx.cs"
 Inherits="DiscussionView" Title="Discussion" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <h3>
  <asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
 </h3>
 <div class="sncore_description">
  <asp:Label CssClass="sncore_description" ID="discussionDescription" runat="server" />
 </div>
 <asp:HyperLink CssClass="sncore_links" id="linkBack" runat="server" Text="&#187; Back" />
 <SnCoreWebControls:PagedList BorderWidth="0px" runat="server" ID="gridManage" AllowCustomPaging="true"
  RepeatColumns="1" RepeatRows="5" RepeatDirection="Horizontal" CssClass="sncore_table"
  ShowHeader="false">
  <pagerstyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" />
  <ItemStyle CssClass="sncore_message_tr_td" />
  <ItemTemplate>
   <div class="sncore_message">
    <div class="sncore_message_subject">
     <div style='<%# (int) Eval("DiscussionThreadCount") > 1 ? "display: none;" : "" %>'>
      <%# Renderer.Render(Eval("Subject")) %>
     </div>
     <div style='<%# (int) Eval("DiscussionThreadCount") > 1 ? "" : "display: none;" %>'>
      <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
       <%# Renderer.Render(Eval("Subject")) %> 
      </a>
     </div>
    </div>
    <div class="sncore_header">
     <span style='<%# (int) Eval("DiscussionThreadCount") > 1 ? "" : "display: none;" %>'>
      <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
       +<%# Eval("DiscussionThreadCount") %> &#187;
      </a>
     </span>
     <%# IsThreaded ? "started" : "posted" %>
     by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
      <%# Renderer.Render(Eval("AccountName")) %>
     </a><span class='<%# (DateTime.UtcNow.Subtract(IsThreaded ? (DateTime) Eval("DiscussionThreadModified") : (DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
      &#187;
      <%# IsThreaded ? "last post" : "" %>
      <%# SessionManager.ToAdjustedString(IsThreaded ? (DateTime)Eval("DiscussionThreadModified") : (DateTime)Eval("Created"))%>
     </span>
    </div>
    <div class='<%# (DateTime.UtcNow.Subtract(IsThreaded ? (DateTime) Eval("DiscussionThreadModified") : (DateTime) Eval("Created")).TotalDays < 3) ? "sncore_content_recent" : "sncore_content" %>'
     style='width: <%# base.OuterWidth - (int) Eval("Level") * 5 %>px'>
     <div class='<%# (DateTime.UtcNow.Subtract(IsThreaded ? (DateTime) Eval("DiscussionThreadModified") : (DateTime) Eval("Created")).TotalDays < 3) ? "sncore_message_body_recent" : "sncore_message_body" %>'>
      <%# IsFull ? Renderer.RenderEx((string)Eval("Body")) : Renderer.GetSummary((string)Eval("Body"))%>
     </div>
    </div>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
</asp:Content>
