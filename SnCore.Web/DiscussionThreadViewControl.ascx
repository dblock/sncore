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
<!-- NOEMAIL-START -->
<input type="hidden" id="posts" value="" />
<script language="javascript">
 function ExpandAll()
 {
  var posts_array = document.getElementById("posts").getAttribute("value").split(";");  
  var i;
  for (i = 0; i < posts_array.length; i++)
  {
   var id = posts_array[i];
   var body_panel = document.getElementById("body_" + id);
   body_panel.style.cssText = "";
   var summary_panel = document.getElementById("summary_" + id);
   summary_panel.style.cssText = "display: none;";
  }
 }
 
 function CollapseExpand(id)
 {
  var body_panel = document.getElementById("body_" + id);
  body_panel.style.cssText = (body_panel.style.cssText == "") ? "display: none;" : "";
  var summary_panel = document.getElementById("summary_" + id);
  summary_panel.style.cssText = (summary_panel.style.cssText == "") ? "display: none;" : "";
 }
</script>
<!-- NOEMAIL-END -->
<div class="sncore_h2sub">
 <asp:HyperLink ID="linkBack" Text="&#187; Back" runat="server" />
 <asp:HyperLink ID="linkNewPosts" runat="server" Text="&#187; New Posts" NavigateUrl="DiscussionThreadsView.aspx" />
 <asp:HyperLink ID="linkAllDiscussions" runat="server" Text="&#187; All Discussions" NavigateUrl="DiscussionsView.aspx" />
 <a href="TellAFriend.aspx?Url=<% Response.Write(Renderer.UrlEncode(Request.Url.PathAndQuery)); %>&Subject=<% Response.Write(Renderer.UrlEncode(Page.Title)); %>">&#187; Tell a Friend</a>     
 <asp:HyperLink ID="linkNew" runat="server" Text="&#187; Post New" />
 <asp:HyperLink ID="linkMove" runat="server" Text="&#187; Move Thread" />
 <a href="#" onclick="ExpandAll();">&#187; Expand All</a>
</div>
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
    <script language="javascript">
     var posts = document.getElementById('posts');
     var posts_value = posts.getAttribute("value");
     if (posts_value.length > 0) posts_value = posts_value + ";";
     posts.setAttribute("value", posts_value + <%# Eval("Id") %>); 
    </script>
    <a name='post_<%# Eval("Id") %>'></a>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
     <tr>
      <td width="<%# (int) Eval("Level") * 20 %>px">
       <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 20 %>px" />
      </td>
      <td align="left" valign="top" width="*" class='<%# GetBodyCssClass((DateTime) Eval("Created")) %>_left_border'>
       <div class="<%# GetSubjectCssClass((int) Eval("Level")) %>">
        <!-- NOEMAIL-START -->
        <a href='#post_<%# Eval("Id") %>' onclick='CollapseExpand(<%# Eval("Id") %>)'>
        <!-- NOEMAIL-END -->
         <%# base.Render(Eval("Subject"))%>
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
       <div id='summary_<%# Eval("Id") %>' style='<%# GetSummaryCssStyle((DateTime) Eval("Created")) %>'>
        <div class="sncore_description">
         <%# base.GetSummary((string) Eval("Body"))%>
         <a href='#post_<%# Eval("Id") %>' onclick='CollapseExpand(<%# Eval("Id") %>)'>&#187;&nbsp;read</a>
        </div>
       </div>
       <div id='body_<%# Eval("Id") %>' style='<%# GetBodyCssStyle((DateTime) Eval("Created")) %>'>
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
      <td width="150" align="center" valign="top" class='<%# GetBodyCssClass((DateTime) Eval("Created")) %>_right_border'>
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
