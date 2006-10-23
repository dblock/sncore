<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionViewControl.ascx.cs"
 Inherits="DiscussionViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<table width="100%">
 <tr>
  <td>
   <asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
  </td>
  <td align="center">
   <asp:Label CssClass="sncore_description" ID="discussionDescription" runat="server" />
  </td>
  <td>
   <link rel="alternate" type="application/rss+xml" title="Rss" href="DiscussionRss.aspx?id=<% Response.Write(DiscussionId); %>" />
   <asp:HyperLink ImageUrl="images/rss.gif" runat="server" ToolTip="Rss" ID="linkRss" />
  </td>
 </tr>
</table>
<div class="sncore_createnew">
 <asp:HyperLink ID="postNew" Text="&#187; Post New" runat="server" />
 <asp:HyperLink ID="linkSearch" Text="&#187; Search" runat="server" />
</div>
<asp:UpdatePanel runat="server" ID="panelThreads" UpdateMode="Always" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedList BorderWidth="0px" CellPadding="4" runat="server" ID="gridManage"
   AllowCustomPaging="true" RepeatColumns="1" RepeatRows="7" RepeatDirection="Horizontal"
   CssClass="sncore_table" ShowHeader="false">
   <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
    prevpagetext="Prev" horizontalalign="Center" />
   <ItemStyle CssClass="sncore_table_tr_td" />
   <ItemTemplate>
    <table class="sncore_message_table" width="100%" cellspacing="0" cellpadding="0">
     <tr>
      <td valign="top" width="28">
       <img src="images/account/discussions.gif" />
      </td>
      <td>
       <div>
        <a class="sncore_message_subject" href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
         <%# base.Render(Eval("Subject")) %>
        </a>
       </div>
       <div class="sncore_link">
        <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
         &#187; read <%# Eval("DiscussionThreadCount") %> post<%# (int) Eval("DiscussionThreadCount") != 1 ? "s" : string.Empty %>
        </a>        
        &#187; last on <%# base.Adjust(Eval("DiscussionThreadModified")).ToString("d")%>
        by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# Renderer.Render(Eval("AccountName")) %></a>
       </div>
      </td>
     </tr>
    </table>
   </ItemTemplate>    
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>


