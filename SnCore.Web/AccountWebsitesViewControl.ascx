<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountWebsitesViewControl.ascx.cs"
 Inherits="AccountWebsitesViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Websites
</div>
<atlas:UpdatePanel ID="panelGrid" runat="server" Mode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountWebsites"
   CssClass="sncore_inner_table" Width="95%" AutoGenerateColumns="false" ShowHeader="false">
   <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
    PrevPageText="Prev" HorizontalAlign="Center" />
   <Columns>
    <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td">
     <itemtemplate>
    <%# Renderer.GetLink(Renderer.Render(Eval("Url")), Renderer.Render(Eval("Name"))) %>
    <div style="font-size: smaller;">
     <%# base.Render(Eval("Description")) %>
    </div>
   </itemtemplate>
    </asp:TemplateColumn>
   </Columns>
  </SnCoreWebControls:PagedGrid>
 </ContentTemplate>
</atlas:UpdatePanel>
