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
   PrevPageText="Prev" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:BoundColumn DataField="CanEdit" Visible="false" />
   <asp:BoundColumn DataField="CanDelete" Visible="false" />
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
       <td align="left" valign="top" width="*" class="sncore_message_left">
        <div class="sncore_message_header">
         <div class="sncore_message_subject">
          <a href="DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&id=<%# Eval("DiscussionThreadId") %>">
           <%# base.Render(Eval("Subject"))%>
          </a>
         </div>
         <div class="sncore_description">
          posted on <%# base.Adjust(Eval("Created")).ToString() %> in
          <a href='DiscussionView.aspx?id=<%# Eval("DiscussionId") %>'>
           <%# base.Render(Eval("DiscussionName"))%>
          </a>
         </div>          
         <div class="sncore_description">
          <a href="DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&id=<%# Eval("DiscussionThreadId") %>">
           &#187; read</a>
          <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&#edit">
           &#187; reply</a>
          <a href="DiscussionPost.aspx?did=<%# Eval("DiscussionId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>&Quote=true&#edit">
           &#187; quote</a>
         </div>
        </div>
        <div class="sncore_message_body">
         <%# base.RenderEx(Eval("Body"))%>
        </div>
       </td>
       <td width="150" align="center" valign="top" class="sncore_message_right">
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
</asp:Panel>
