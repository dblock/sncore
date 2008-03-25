<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountWebsitesViewControl.ascx.cs"
 Inherits="AccountWebsitesViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Websites
</div>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountWebsites" ShowHeader="false"
   AllowCustomPaging="true" AutoGenerateColumns="false" CssClass="sncore_account_table">
   <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next"
    PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:TemplateColumn ItemStyle-Width="150">
      <ItemTemplate>
       <img src='AccountWebsitePictureThumbnail.aspx?id=<%# Eval("Id") %>' alt='<%# Renderer.Render(Eval("Name")) %>' />
      </ItemTemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <ItemTemplate>
       <div class="sncore_h2left">
        <%# Renderer.GetLink(Renderer.Render(Eval("Url")), Renderer.Render(Eval("Name"))) %>
        <div class="sncore_link">
         <a href='<%# Renderer.Render(Eval("Url")) %>'><%# Renderer.Render(Eval("Url")) %></a>
        </div>
       </div>
       <div class="sncore_description">
        <%# base.Render(Eval("Description")) %>
       </div>
     </ItemTemplate>
    </asp:TemplateColumn>
   </Columns>
  </SnCoreWebControls:PagedGrid>
 </ContentTemplate>
</asp:UpdatePanel>
