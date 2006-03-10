<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountFeedItemsControl.ascx.cs"
 Inherits="SearchAccountFeedItemsControl" %>
<%@Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelFeedItemsResults" runat="server">
 <div class="sncore_h2">
  Feeds
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
    <itemtemplate>
     <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
      <div>
       <%# base.Render(Eval("AccountName")) %>
      </div>
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <span>
      <div class="sncore_h2left">
       <a target="_blank" href='<%# base.Render(Eval("Link")) %>'>
        <%# base.Render(Eval("Title")) %>
       </a>
      </div>
      <div class="sncore_h2sub">
       in
       <a href='AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>'>
        <%# base.Render(Eval("AccountFeedName")) %>
       </a>
      </div>
      <div>
       <%# Renderer.CleanHtml(Eval("Description")) %>
      </div>
     </span>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
