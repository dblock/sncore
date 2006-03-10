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
      <td width="150" align="center" valign="top">
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
        <br />
        <b>
         <%# base.Render(Eval("AccountName")) %>
        </b></a>
      </td>
      <td align="left" valign="top" width="*">
       <b>subject:</b>
       <%# base.Render(Eval("Subject"))%>
       <br />
       <b>posted:</b>
       <%# base.Adjust(Eval("Created")).ToString() %>
       <br />
       <br />
       <%# base.RenderEx(Eval("Body"))%>
      </td>
     </tr>
    </table>
   </ItemTemplate>
  </asp:TemplateColumn>
  <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Center">
   <ItemTemplate>
    <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&#edit">
     Reply</a>    
    <hr noshade width="30px" size="1px" />
    <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true&#edit">
     Quote</a>
   </ItemTemplate>
  </asp:TemplateColumn>
  <asp:ButtonColumn Text="Edit" CommandName="Edit" ItemStyle-CssClass="sncore_table_tr_td"
   ItemStyle-HorizontalAlign="Center" />
  <asp:ButtonColumn CommandName="Delete" ItemStyle-CssClass="sncore_table_tr_td"
   ItemStyle-HorizontalAlign="Center" Text="Delete" />
 </Columns>
</asp:DataGrid>
