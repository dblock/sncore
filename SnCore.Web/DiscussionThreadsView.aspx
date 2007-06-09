<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="DiscussionThreadsView.aspx.cs" Inherits="DiscussionThreadsView" Title="New Posts" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <SnCore:Title ID="titleDiscussions" Text="New Posts" runat="server">  
     <Template>
      <div class="sncore_title_paragraph">
       These are the most recent discussion posts.
      </div>
      <div class="sncore_title_paragraph">
       Don't be shy. Exchange views, opinions and essential information with other members. 
       Create controversy. Post to a discussion.
      </div>
      <div class="sncore_title_paragraph">
       Everyone can read your posts, but one must be a registered member with a verified e-mail address to start a new topic
       or reply. Click <a href="AccountCreate.aspx">here to join</a> or <a href="AccountLogin.aspx">here to login</a> if you're
       already a member.
      </div>
     </Template>
    </SnCore:Title>
   </td>
   <td align="right">
    <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="DiscussionThreadsRss.aspx" />
   </td>
  </tr>
 </table>
 <div class="sncore_h2sub">
  <a href="DiscussionsView.aspx">&#187; All Discussions</a>
  <a href="DiscussionTopOfThreadsView.aspx">&#187; New Threads</a>
  <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
 </div>
 <asp:UpdatePanel runat="server" ID="panelThreads" UpdateMode="Always" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList BorderWidth="0px" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="1" RepeatRows="5" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_message_tr_td" />
    <ItemTemplate>
     <div class="sncore_message">
      <div class="sncore_message_subject">
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
       posted 
       by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# Renderer.Render(Eval("AccountName")) %></a>
       in <a href='DiscussionView.aspx?id=<%# Eval("DiscussionId") %>'><%# Renderer.Render(Eval("DiscussionName")) %></a>
       <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
        &#187; <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>
       </span>
      </div>
      <div class="sncore_content">
       <div class="sncore_message_body">
        <%# RenderEx(Eval("Body")) %>
       </div>
      </div>
      <div class="sncore_footer">
       <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>">&#187; read</a>
       <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&#edit">
        &#187; reply</a>
       <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true&#edit">
        &#187; quote</a>
      </div>
     </div>
    </ItemTemplate>    
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
