<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchDiscussionPostsControl.ascx.cs"
 Inherits="SearchDiscussionPostsControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelDiscussionPostsResults" runat="server">
 <div class="sncore_h2">
  Discussion Posts
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridResults" PageSize="5"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false" BorderWidth="0">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:BoundColumn DataField="CanEdit" Visible="false" />
   <asp:BoundColumn DataField="CanDelete" Visible="false" />
   <asp:TemplateColumn ItemStyle-CssClass="sncore_message_tr_td">
    <itemtemplate>
     <div class="sncore_message">
      <div class="sncore_message_subject">
       <a href='DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&id=<%# Eval("DiscussionThreadId") %>&ReturnUrl=<%# SnCore.Tools.Web.Renderer.UrlEncode(Request.Url.PathAndQuery) %>'>
        <%# base.Render(Eval("Subject"))%>
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
       in 
       <a href='DiscussionView.aspx?id=<%# Eval("DiscussionId") %>'>
        <%# base.Render(Eval("DiscussionName"))%>
       </a>
       <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
        &#187; <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>
       </span>
      </div>
      <div class="sncore_content" style='width: <%# 680 - (int) Eval("Level") * 10 %>px'>
       <div class="sncore_message_body">
        <%# RenderEx(Eval("Body")) %>
       </div>
      </div>
      <div class="sncore_footer">
       <a href="DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&id=<%# Eval("DiscussionThreadId") %>">
        &#187; read</a>
       <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
        &#187; reply</a>
       <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true">
        &#187; quote</a>
      </div>
     </div>      
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
