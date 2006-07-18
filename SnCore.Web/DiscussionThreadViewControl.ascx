<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionThreadViewControl.ascx.cs"
 Inherits="DiscussionThreadViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<table width="100%">
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
 <a href="DiscussionThreadsView.aspx">&#187; New Posts</a>
 <a href="DiscussionsView.aspx">&#187; All Forums</a>
 <asp:HyperLink ID="linkMove" runat="server" Text="&#187; Move Thread" />
</div>
<SnCoreWebControls:PagedGrid CellPadding="4" ShowHeader="false" runat="server" ID="discussionThreadView"
 AutoGenerateColumns="false" CssClass="sncore_table" BorderWidth="0" OnItemDataBound="discussionThreadView_ItemDataBound"
 OnItemCommand="discussionThreadView_ItemCommand">
 <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
 <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
 <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
  PrevPageText="Prev" HorizontalAlign="Center" />
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:BoundColumn DataField="CanEdit" Visible="false" />
  <asp:BoundColumn DataField="CanDelete" Visible="false" />
  <asp:TemplateColumn>
   <itemtemplate>
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
       <td width="<%# (int) Eval("Level") * 20 %>px">
        <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 20 %>px" />
       </td>
       <td align="left" valign="top" width="*" class="sncore_message_left_border">
        <div class="sncore_message_subject">
         <%# base.Render(Eval("Subject"))%>
        </div>
        <div class="sncore_description">
         posted <%# base.Adjust(Eval("Created")).ToString() %>
        </div>
        <div class="sncore_description">
         <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&#edit">
          &#187; reply</a>
         <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true&#edit">
          &#187; quote</a>
         <a id="linkEdit" runat="server">
          &#187; edit</a>
         <asp:LinkButton CommandName="Delete" id="linkDelete" runat="server" Text="&#187; delete" CommandArgument='<%# Eval("Id") %>'
          OnClientClick="return confirm('Are you sure you want to do this?')" />
        </div>
        <div class="sncore_message_body">
         <%# base.RenderEx(Eval("Body"))%>
        </div>
       </td>
       <td width="150" align="center" valign="top" class="sncore_message_right_border">
        <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
         <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" style="<%# (((string) Eval("Body")).Length < 64) ? "height:50px;" : "" %>" />
        </a>
        <div class="sncore_link_description">
         <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
          <%# base.Render(Eval("AccountName")) %>
         </a>
        </div>
       </td>
      </tr>
    </table>
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</SnCoreWebControls:PagedGrid>
