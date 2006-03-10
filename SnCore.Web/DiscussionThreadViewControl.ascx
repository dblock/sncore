<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionThreadViewControl.ascx.cs" Inherits="DiscussionThreadViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<br />
<asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
<br />
<asp:Label CssClass="sncore_h2sub" ID="discussionDescription" runat="server" />
<br />
<asp:HyperLink ID="linkBack" Text="Back" CssClass="sncore_createnew" runat="server" />
<%@Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCoreWebControls:PagedGrid CellPadding="4" ShowHeader="false" runat="server" ID="discussionThreadView" AutoGenerateColumns="false"
 CssClass="sncore_table" OnItemDataBound="discussionThreadView_ItemDataBound" OnItemCommand="discussionThreadView_ItemCommand">
   <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next" PrevPageText="Prev"
  		HorizontalAlign="Center" />
<Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:BoundColumn DataField="CanEdit" Visible="false" />
  <asp:BoundColumn DataField="CanDelete" Visible="false" />
  <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Center">
   <ItemTemplate>
    <table width="100%">
     <tr>
      <td style="width: <%# (int) Eval("Level") * 20 %>px;">
       <img alt="" src="images/Spacer.gif" style="width: <%# (int) Eval("Level") * 20 %>px" />
      </td>
      <td width="150" align="center" valign="top">
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <img alt="" border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>">
        <div>
         <%# base.Render(Eval("AccountName")) %>
        </div>
       </a>
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
    <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>#edit">
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
</SnCoreWebControls:PagedGrid>
