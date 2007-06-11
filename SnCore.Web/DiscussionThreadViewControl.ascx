<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionThreadViewControl.ascx.cs"
 Inherits="DiscussionThreadViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<table class="sncore_title_table">
 <tr>
  <td>
   <asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
  </td>
  <td>
   <asp:Label CssClass="sncore_description" ID="discussionDescription" runat="server" />
  </td>
 </tr>
</table>
<div class="sncore_h2sub">
 <asp:HyperLink ID="linkBack" Text="&#187; Back" runat="server" />
 <asp:HyperLink ID="linkNewPosts" runat="server" Text="&#187; New Posts" NavigateUrl="DiscussionThreadsView.aspx" />
 <asp:HyperLink ID="linkAllDiscussions" runat="server" Text="&#187; All Discussions" NavigateUrl="DiscussionsView.aspx" />
 <a href="TellAFriend.aspx?Url=<% Response.Write(Renderer.UrlEncode(Request.Url.PathAndQuery)); %>&Subject=<% Response.Write(Renderer.UrlEncode(Page.Title)); %>">&#187; Tell a Friend</a>     
 <asp:HyperLink ID="linkNew" runat="server" Text="&#187; Post New" />
 <asp:HyperLink ID="linkMove" runat="server" Text="&#187; Move Thread" />
</div>
<SnCoreWebControls:PagedGrid BorderColor="White" ShowHeader="false" runat="server" ID="discussionThreadView"
 AutoGenerateColumns="false" CssClass="sncore_table" BorderWidth="0" OnItemDataBound="discussionThreadView_ItemDataBound"
 OnItemCommand="discussionThreadView_ItemCommand" AllowPaging="false" AllowCustomPaging="false">
 <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
  PrevPageText="Prev" HorizontalAlign="Center" />
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:BoundColumn DataField="CanEdit" Visible="false" />
  <asp:BoundColumn DataField="CanDelete" Visible="false" />
  <asp:TemplateColumn>
   <itemtemplate>
    <span class="sncore_message_tr_td_span" style='margin-left: <%# (int) Eval("Level") * 10 %>px'>
     <div class="sncore_message">
      <div class="sncore_message_subject" style='<%# (int) Eval("Level") != 0 ? "display: none;" : ""%>'>
       <%# base.Render(Eval("Subject")) %>
      </div>
      <div class="sncore_person">
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" style="width: 50px;"/>
       </a>
      </div>
      <div class="sncore_header">
       posted 
       by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# Renderer.Render(Eval("AccountName")) %></a>
       <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
        &#187; <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>
       </span>
      </div>
      <div class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_content_recent" : "sncore_content" %>'
       style='width: <%# 680 - (int) Eval("Level") * 10 %>px'>
       <div class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_message_body_recent" : "sncore_message_body" %>'>
        <%# RenderEx(Eval("Body")) %>
       </div>
      </div>
      <div class="sncore_footer">
       <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&#edit">
        &#187; reply</a>
       <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true&#edit">
        &#187; quote</a>
       <a id="linkEdit" runat="server">
        &#187; edit</a>
       <asp:LinkButton CommandName="Delete" id="linkDelete" runat="server" Text="&#187; delete" CommandArgument='<%# Eval("Id") %>'
        OnClientClick="return confirm('Are you sure you want to do this?')" />
      </div>
     </div>      
    </span>
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</SnCoreWebControls:PagedGrid>
