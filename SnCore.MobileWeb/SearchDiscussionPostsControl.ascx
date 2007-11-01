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
   PrevPageText="Prev" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-CssClass="sncore_message_tr_td">
    <itemtemplate>
     <div class="sncore_message">
      <div class="sncore_message_subject">
       <div>
        <a href="DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>&did=<%# Eval("DiscussionId") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
         <%# Renderer.Render(Eval("Subject")) %> 
        </a>
       </div>
      </div>
      <div class="sncore_header">
       posted by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
        <%# Renderer.Render(Eval("AccountName")) %>
       </a><span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
        &#187;
        <%# SessionManager.ToAdjustedString((DateTime)Eval("Created"))%>
       </span>
      </div>
      <div class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_content_recent" : "sncore_content" %>'
       style='width: <%# base.OuterWidth - (int) Eval("Level") * 5 %>px'>
       <div class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_message_body_recent" : "sncore_message_body" %>'>
        <%# SessionManager.RenderMarkups(Renderer.RenderEx((string)Eval("Body"))) %>
       </div>
      </div>
     </div>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
