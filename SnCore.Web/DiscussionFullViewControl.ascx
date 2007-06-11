<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionFullViewControl.ascx.cs"
 Inherits="DiscussionFullViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<div class="sncore_h2">
 <asp:Label ID="discussionLabel" runat="server" />
</div>
<div class="sncore_h2sub" runat="server" id="divDescription">
 <asp:Label ID="discussionDescription" runat="server" />
</div>
<div class="sncore_cancel">
 <asp:HyperLink ID="postNew" Text="Post New" runat="server" />
</div>
<asp:DataGrid ShowHeader="false" CellPadding="4" runat="server" ID="discussionView" AutoGenerateColumns="false" BorderWidth="0" BorderColor="White"
 CssClass="sncore_inner_table" Width="95%" OnItemDataBound="discussionView_ItemDataBound" OnItemCommand="discussionView_ItemCommand">
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:BoundColumn DataField="CanEdit" Visible="false" />
  <asp:BoundColumn DataField="CanDelete" Visible="false" />
  <asp:TemplateColumn>
   <ItemTemplate>
    <span class="sncore_message_tr_td_span" style='margin-left: <%# (int) Eval("Level") * 10 %>px'>
     <div class="sncore_message">
      <div class="sncore_message_subject" style='<%# (int) Eval("Level") != 0 || (string) Eval("Subject") == "Untitled" ? "display: none;" : ""%>'>
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
       style='width: <%# base.OuterWidth - (int) Eval("Level") * 10 %>px'>
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
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
