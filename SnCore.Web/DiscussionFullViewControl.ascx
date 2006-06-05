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
<asp:DataGrid ShowHeader="false" CellPadding="4" runat="server" ID="discussionView" AutoGenerateColumns="false"
 CssClass="sncore_inner_table" Width="95%" OnItemDataBound="discussionView_ItemDataBound" OnItemCommand="discussionView_ItemCommand">
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:BoundColumn DataField="CanEdit" Visible="false" />
  <asp:BoundColumn DataField="CanDelete" Visible="false" />
  <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Center">
   <ItemTemplate>
    <table width="100%">
     <tr>
      <td width="<%# (int) Eval("Level") * 20 %>px">
       <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 20 %>px" />
      </td>
      <td align="left" valign="top" width="*">
       <div><b>subject:</b> <%# base.Render(Eval("Subject"))%></div>
       <div><b>posted:</b> <%# base.Adjust(Eval("Created")).ToString() %></div>
       <div style="margin: 10px 0px 10px 0px;">
        <%# base.RenderEx(Eval("Body"))%>
       </div>
      </td>
      <td width="150" align="center" valign="top">
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
        <div class="sncore_description">
         <%# base.Render(Eval("AccountName")) %>
        </div>
       </a>
      </td>
     </tr>
     <tr>
      <td width="<%# (int) Eval("Level") * 20 %>px">
       <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 20 %>px" />
      </td>
      <td colspan="2" style="font-size: smaller; text-align: left; padding-left: 10px;">
       <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&#edit">
        &#187; reply</a>
       <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true&#edit">
        &#187; quote</a>
       <a id="linkEdit" runat="server">
        &#187; edit</a>
       <asp:LinkButton CommandName="Delete" id="linkDelete" runat="server" Text="&#187; delete" />
      </td>
     </tr>
    </table>
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
