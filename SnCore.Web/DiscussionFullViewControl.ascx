<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionFullViewControl.ascx.cs"
 Inherits="DiscussionFullViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<br />
<asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
<br />
<asp:Label CssClass="sncore_h2sub" ID="discussionDescription" runat="server" />
<br />
<asp:HyperLink ID="postNew" Text="Post New" CssClass="sncore_createnew" runat="server" />
<asp:DataGrid ShowHeader="false" CellPadding="4" runat="server" ID="discussionView" AutoGenerateColumns="false" BorderWidth="0"
 CssClass="sncore_inner_table" Width="95%" OnItemDataBound="discussionView_ItemDataBound" OnItemCommand="discussionView_ItemCommand">
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:BoundColumn DataField="CanEdit" Visible="false" />
  <asp:BoundColumn DataField="CanDelete" Visible="false" />
  <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Center">
   <ItemTemplate>
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
       <td width="<%# (int) Eval("Level") * 20 %>px">
        <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 20 %>px" />
       </td>
       <td align="left" valign="top" width="*" class="sncore_message_left_border">
        <div class="sncore_message_header">
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
          <asp:LinkButton CommandName="Delete" id="linkDelete" runat="server" Text="&#187; delete" OnClientClick="return confirm('Are you sure you want to do this?')" />
         </div>
         <div class="sncore_message_body">
          <%# base.RenderEx(Eval("Body"))%>
         </div>
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
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
