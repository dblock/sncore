<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountGroupFeaturedViewControl.ascx.cs"
 Inherits="AccountGroupFeaturedViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="FeaturedAccountGroupsRss.aspx" 
 Title="Featured Groups" ButtonVisible="false" />
<div id="divTitle" class="sncore_h2" runat="server">
 <a href='AccountGroupsView.aspx'>
  Featured Groups
  <img src="images/site/right.gif" border="0" />
 </a>
</div>
<asp:Panel ID="panelLinks" runat="server" CssClass="sncore_createnew">
 <div id="divLinks" class="sncore_link" runat="server">
  <span id="spanLinkViewGroup" runat="server">
   <a href='AccountGroupsView.aspx'>
    &#187; all groups
   </a>
   <a href='FeaturedAccountGroupsRss.aspx'>
    &#187; rss
   </a>
  </span>
 </div>
</asp:Panel>
<asp:DataGrid CellPadding="2" runat="server" ID="gridManage" PageSize="3"
 AllowPaging="false" AutoGenerateColumns="false" CssClass="sncore_half_table" ShowHeader="false">
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:TemplateColumn  ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
   <itemtemplate>
    <a href='<%# string.Format("AccountGroupView.aspx?id={0}", Eval("DataRowId")) %>'>
     <img border="0" src='<%# string.Format("AccountGroupPictureThumbnail.aspx?id={0}", GetAccountGroup((int) Eval("DataRowId")).PictureId) %>' />
    </a>
   </itemtemplate>
  </asp:TemplateColumn>
  <asp:TemplateColumn ItemStyle-VerticalAlign="Top">
   <itemtemplate>
    <div class="sncore_title">
     <a href="<%# string.Format("AccountGroupView.aspx?id={0}", Eval("DataRowId")) %>">
      <%# Renderer.Render(GetAccountGroup((int) Eval("DataRowId")).Name) %>
     </a>
    </div>
    <div class="sncore_description">
      <%# Renderer.GetSummary(GetAccountGroup((int)Eval("DataRowId")).Description) %>
    </div>
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
