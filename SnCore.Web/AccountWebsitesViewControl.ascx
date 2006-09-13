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
  <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="accountWebsites" 
   AllowCustomPaging="true">
   <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next"
    PrevPageText="Prev" HorizontalAlign="Center" />
    <ItemTemplate>
     <div class="sncore_h2left">
      <%# Renderer.GetLink(Renderer.Render(Eval("Url")), Renderer.Render(Eval("Name"))) %>
     </div>
     <div class="sncore_description">
      <%# base.Render(Eval("Description")) %>
     </div>
   </itemtemplate>
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</atlas:UpdatePanel>
