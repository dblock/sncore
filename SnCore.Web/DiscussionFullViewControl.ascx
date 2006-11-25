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
<!-- NOEMAIL-START -->
<script language="javascript">
 function CollapseExpand(id)
 {
  var panel = document.getElementById("body_" + id);
  panel.style.cssText = (panel.style.cssText == "") ? "display: none;" : "";
 }
</script>
<!-- NOEMAIL-END -->
<div class="sncore_cancel">
 <asp:HyperLink ID="postNew" Text="Post New" runat="server" />
</div>
<asp:DataGrid ShowHeader="false" CellPadding="4" runat="server" ID="discussionView" AutoGenerateColumns="false" BorderWidth="0" BorderColor="White"
 CssClass="sncore_inner_table" Width="95%" OnItemDataBound="discussionView_ItemDataBound" OnItemCommand="discussionView_ItemCommand">
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:BoundColumn DataField="CanEdit" Visible="false" />
  <asp:BoundColumn DataField="CanDelete" Visible="false" />
  <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Center">
   <ItemTemplate>
    <a name='post_<%# Eval("Id") %>'></a>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
     <tr>
      <td width="<%# (int) Eval("Level") * 20 %>px">
       <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 20 %>px" />
      </td>
      <td align="left" valign="top" width="*" class='<%# GetCssClass((DateTime) Eval("Created")) %>_left_border'>
       <div class="sncore_message_header">
        <div class="sncore_message_subject">
         <!-- NOEMAIL-START -->
         <a href='#post_<%# Eval("Id") %>' onclick='CollapseExpand(<%# Eval("Id") %>)'>
         <!-- NOEMAIL-END -->
          <%# base.Render((string) Eval("Subject"))%>
         <!-- NOEMAIL-START -->
         </a>
         <!-- NOEMAIL-END -->
        </div>
        <div class="sncore_description">
         posted <%# base.Adjust(Eval("Created")).ToString() %>
        </div>
        <div id='body_<%# Eval("Id") %>' style='<%# GetCssStyle((DateTime) Eval("Created")) %>'>
         <div class="sncore_description">
          <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(base.ReturnUrl) %>&#edit">
           &#187; reply</a>
          <a href="DiscussionPost.aspx?did=<%# base.DiscussionId %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(base.ReturnUrl) %>&Quote=true&#edit">
           &#187; quote</a>
          <a id="linkEdit" runat="server">
           &#187; edit</a>
          <asp:LinkButton CommandName="Delete" id="linkDelete" runat="server" Text="&#187; delete" OnClientClick="return confirm('Are you sure you want to do this?')" />
         </div>
         <%# base.RenderEx(Eval("Body"))%>
        </div>
       </div>
      </td>
      <td width="150" align="center" valign="top" class='<%# GetCssClass((DateTime) Eval("Created")) %>_right_border'>
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" style='<%# GetCssPictureStyle((DateTime) Eval("Created"), ((string) Eval("Body")).Length) %>' />
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
