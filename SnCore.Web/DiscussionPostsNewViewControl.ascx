<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionPostsNewViewControl.ascx.cs"
 Inherits="DiscussionPostsNewViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
   <div class="sncore_h2">
    <a href="DiscussionsView.aspx">
      New Discussion Posts
     <img src="images/site/right.gif" border="0" />
    </a>
   </div>
  </td>
 </tr>
 <tr>
  <td>
   <div class="sncore_createnew">
    <div class="sncore_link">
     <a href="DiscussionsView.aspx">&#187; post new</a>
     <a href="DiscussionTopOfThreadsView.aspx">&#187; new threads</a>
     <a href="DiscussionThreadsView.aspx">&#187; new posts</a>
     <a href="DiscussionsView.aspx">&#187; all</a>
     <a href="DiscussionThreadsRss.aspx">&#187; rss</a>
    </div>
   </div>
  </td>
 </tr>
</table>
<SnCore:RssLink ID="linkRelThreadsRss" runat="server" NavigateUrl="DiscussionThreadsRss.aspx" 
 Title="New Discussion Posts" ButtonVisible="false" />
<SnCoreWebControls:PagedList BorderWidth="0px" runat="server" ID="discussionView"
 RepeatColumns="1" RepeatRows="5" AllowCustomPaging="true" RepeatDirection="Horizontal"
 CssClass="sncore_half_table" ShowHeader="false">
 <ItemStyle CssClass="sncore_message_tr_td_halftable" />
 <ItemTemplate>
  <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl='<%# string.Format("DiscussionRss.aspx?id={0}", Eval("DiscussionId")) %>' 
   ButtonVisible="false" Title='<%# base.Render(Eval("DiscussionName"))%>' />
  <div class="sncore_message">
   <div>
    <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
     <%# base.Render(Eval("Subject")) %>
    </a>
   </div>
   <div class="sncore_person">
    <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" style="width: 50px;"/>
    </a>
   </div>
   <div class="sncore_header">
    posted by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# Renderer.Render(Eval("AccountName")) %></a>
    in 
    <a href='DiscussionView.aspx?id=<%# Eval("DiscussionId") %>'>
     <%# base.Render(Eval("DiscussionName"))%> 
    </a>
   </div>
   <div class="sncore_content_halftable">
    <div class="sncore_message_body_halftable">
     <%# GetSummary((string) Eval("Body")) %>
    </div>
   </div>
   <div class="sncore_footer">
    <%# SessionManager.ToAdjustedString((DateTime) Eval("DiscussionThreadModified")) %>
    <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
     &#187; read
    </a>    
   </div>
  </div>
 </ItemTemplate>    
</SnCoreWebControls:PagedList>

