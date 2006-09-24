<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountBlogPostsControl.ascx.cs"
 Inherits="SearchAccountBlogPostsControl" %>
<%@Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelBlogPostsResults" runat="server">
 <div class="sncore_h2">
  Blog Posts
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedGrid CellPadding="0" BorderWidth="0" runat="server" ID="gridResults" PageSize="5"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Top">
    <ItemTemplate>
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
       <td align="left" valign="top" width="*" class="sncore_message_table">
        <div class="sncore_message_header">
         <div class="sncore_message_subject">
          <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
           <%# base.Render(GetValue(Eval("Title"), "Untitled")) %>
          </a>
         </div>
         <div class="sncore_description">
          by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
           <%# base.Render(Eval("AccountName")) %>
          </a>
          on 
          <%# base.Adjust(Eval("Created")) %>
          <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
           &#187; <%# GetComments((int) Eval("CommentCount"))%></a>
         </div>
        </div>
        <div class="sncore_message_body">
         <%# base.RenderEx(Eval("Body")) %>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
