<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionViewControl.ascx.cs"
 Inherits="DiscussionViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<table width="100%">
 <tr>
  <td>
   <asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
  </td>
  <td align="center">
   <asp:Label CssClass="sncore_description" ID="discussionDescription" runat="server" />
  </td>
  <td>
   <SnCore:RssLink ID="linkRelRss" runat="server" />
  </td>
 </tr>
</table>
<div class="sncore_createnew">
 <asp:HyperLink ID="postNew" Text="&#187; Post New" runat="server" />
 <asp:LinkButton ID="linkSearch" runat="server" Text="&#187; Search" CausesValidation="false" />
</div>
<ajaxtoolkit:CollapsiblePanelExtender ID="panelSearchExtender" runat="server"
 TargetControlID="panelSearch" Collapsed="true" ExpandedSize="75"
 ExpandControlID="linkSearch" CollapseControlID="linkSearch" SuppressPostBack="true">
</ajaxtoolkit:CollapsiblePanelExtender>
<asp:Panel ID="panelSearch" runat="server" CssClass="sncore_collapsed_div">
 <table runat="server" id="tableSearch" class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    search:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="search" runat="server" Text="Search!" CssClass="sncore_form_button"
     OnClick="search_Click" />
   </td>
  </tr>
 </table>
</asp:Panel>
<asp:UpdatePanel runat="server" ID="panelThreads" UpdateMode="Always" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedList BorderWidth="0px" runat="server" ID="gridManage"
   AllowCustomPaging="true" RepeatColumns="1" RepeatRows="7" RepeatDirection="Horizontal"
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
       <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>&width=75&height=75"/>
      </a>
     </div>
     <div class="sncore_header">
      <%# IsThreaded ? "started" : "posted" %>
      by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# Renderer.Render(Eval("AccountName")) %></a>
      <span class='<%# (DateTime.UtcNow.Subtract(IsThreaded ? (DateTime) Eval("DiscussionThreadModified") : (DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
       &#187; 
       <%# IsThreaded ? "last post" : "" %>
       <%# SessionManager.ToAdjustedString(IsThreaded ? (DateTime)Eval("DiscussionThreadModified") : (DateTime)Eval("Created"))%>
      </span>
     </div>
     <div class='<%# (DateTime.UtcNow.Subtract(IsThreaded ? (DateTime) Eval("DiscussionThreadModified") : (DateTime) Eval("Created")).TotalDays < 3) ? "sncore_content_recent" : "sncore_content" %>'
      style='width: <%# base.OuterWidth - (int) Eval("Level") * 10 %>px'>
      <div class='<%# (DateTime.UtcNow.Subtract(IsThreaded ? (DateTime) Eval("DiscussionThreadModified") : (DateTime) Eval("Created")).TotalDays < 3) ? "sncore_message_body_recent" : "sncore_message_body" %>'>
      <%# IsFull ? Renderer.RenderEx((string)Eval("Body")) : GetSummary((string)Eval("Body"))%>
     </div>
     </div>
     <div class="sncore_footer">
      <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
       &#187; reply</a>
      <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true">
       &#187; quote</a>
      <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
       &#187; <%# IsThreaded && (int) Eval("DiscussionThreadCount") > 1 ? string.Format("{0} post{1}", Eval("DiscussionThreadCount"), (int)Eval("DiscussionThreadCount") != 1 ? "s" : string.Empty) : "read thread"%>
      </a>
     </div>
    </div>
   </ItemTemplate>    
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>


