<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionThreadViewControl.ascx.cs"
 Inherits="DiscussionThreadViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
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
 <a href="DiscussionsView.aspx">&#187; All Discussions</a>
 <a href="TellAFriend.aspx?Url=<% Response.Write(Renderer.UrlEncode(Request.Url.PathAndQuery)); %>&Subject=<% Response.Write(Renderer.UrlEncode(Page.Title)); %>">&#187; Tell a Friend</a>     
 <asp:HyperLink ID="linkNew" runat="server" Text="&#187; Post New" />
 <asp:HyperLink ID="linkMove" runat="server" Text="&#187; Move Thread" />
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
<SnCoreWebControls:PagedGrid CellPadding="4" BorderColor="White" ShowHeader="false" runat="server" ID="discussionThreadView"
 AutoGenerateColumns="false" CssClass="sncore_table" BorderWidth="0" OnItemDataBound="discussionThreadView_ItemDataBound"
 OnItemCommand="discussionThreadView_ItemCommand" AllowPaging="false" AllowCustomPaging="false">
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
    <a name='post_<%# Eval("Id") %>'></a>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
     <tr>
      <td width="<%# (int) Eval("Level") * 20 %>px">
       <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 20 %>px" />
      </td>
      <td align="left" valign="top" width="*" class='<%# GetCssClass((DateTime) Eval("Created")) %>_left_border'>
       <div class="sncore_message_subject">
        <!-- NOEMAIL-START -->
        <a href='#post_<%# Eval("Id") %>' onclick='CollapseExpand(<%# Eval("Id") %>)'>
        <!-- NOEMAIL-END -->
         <%# base.Render(GetSubject((string) Eval("Subject")))%>
        <!-- NOEMAIL-START -->
        </a>
        <!-- NOEMAIL-END -->          
       </div>
       <div class="sncore_description">
        posted by
        <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
         <%# base.Render(Eval("AccountName")) %>
        </a>
        on       
        <%# base.Adjust(Eval("Created")).ToString() %>
       </div>
       <div id='body_<%# Eval("Id") %>' style='<%# GetCssStyle((DateTime) Eval("Created")) %>'>
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
       </div>
      </td>
      <td width="150" align="center" valign="top" class='<%# GetCssClass((DateTime) Eval("Created")) %>_right_border'>
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" style='<%# GetCssPictureStyle((DateTime) Eval("Created"), ((string) Eval("Body")).Length) %>' />
       </a>
      </td>
     </tr>
   </table>
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</SnCoreWebControls:PagedGrid>
