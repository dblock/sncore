<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountBlogPostsControl.ascx.cs"
 Inherits="SearchAccountBlogPostsControl" %>
<%@Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelBlogPostsResults" runat="server">
 <div class="sncore_h2">
  Blog Posts
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridResults" PageSize="10"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Top">
    <ItemTemplate>
     <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
      <div>
       <%# base.Render(Eval("AccountName")) %>
      </div>
     </a>
    </ItemTemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <ItemTemplate>
     <span>
      <div class="sncore_h2left">
       <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
        <%# base.Render(Eval("Title")) %>
       </a>
      </div>
      <div class="sncore_h2sub">
       in
       <a href='AccountBlogView.aspx?id=<%# Eval("AccountBlogId") %>'>
        <%# base.Render(Eval("AccountBlogName")) %>
       </a>
      </div>
      <div>
       <%# Renderer.CleanHtml(Eval("Body")) %>
      </div>
     </span>
    </ItemTemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
