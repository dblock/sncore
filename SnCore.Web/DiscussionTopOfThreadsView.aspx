<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="DiscussionTopOfThreadsView.aspx.cs" Inherits="DiscussionTopOfThreadsView" Title="New Discussion Threads" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <link rel="alternate" type="application/rss+xml" title="Rss" href="DiscussionTopOfThreadsRss.aspx">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkDiscussionThreads" Text="New Posts"
   runat="server" />
 </div>
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     New Threads
    </div>
   </td>
   <td align="right">
    <a href="DiscussionTopOfThreadsRss.aspx">
     <img border="0" src="images/rss.gif" alt="Rss" />
    </a>
   </td>
  </tr>
 </table>
 <div class="sncore_h2sub">
  <a href="DiscussionsView.aspx">&#187; All Forums</a>
  <a href="DiscussionThreadsView.aspx">&#187; New Posts</a>
  <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
 </div>
 <atlas:UpdatePanel runat="server" ID="panelThreads" Mode="Always" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList BorderWidth="0px" CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="1" RepeatRows="5" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_table_tr_td" Width="25%" />
    <ItemTemplate>
     <table class="sncore_message_table" width="100%" cellspacing="0" cellpadding="0">
      <tr>
       <td width="18">
        <img src="images/account/discussions.gif" />
       </td>
       <td>
        <div>
         <a class="sncore_message_subject" href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
          <%# base.Render(Eval("Subject")) %>
         </a>
        </div>
        <div class="sncore_link">
         <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>">&#187; read</a>
         &#187; posted on <%# base.Adjust(Eval("Created")).ToString("d") %>
         <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&#edit">
          &#187; reply</a>
         <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true&#edit">
          &#187; quote</a>
         by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# Renderer.Render(Eval("AccountName")) %></a>
         in <a href='DiscussionView.aspx?id=<%# Eval("DiscussionId") %>'><%# Renderer.Render(Eval("DiscussionName")) %></a>
        </div>
       </td>
       <td width="150" align="center" valign="top" class="sncore_message_right">
        <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
         <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" style="height:50px;" />
        </a>
        <div class="sncore_link_description">
         <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
          <%# base.Render(Eval("AccountName")) %>
         </a>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>    
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </atlas:UpdatePanel>
</asp:Content>
