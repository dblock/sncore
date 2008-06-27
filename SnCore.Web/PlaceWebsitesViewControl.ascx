<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlaceWebsitesViewControl.ascx.cs"
 Inherits="PlaceWebsitesViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<SnCore:Title ID="titleNewWebsite" Text="Websites" runat="server" ExpandedSize="75">  
 <Template>
  <div class="sncore_title_paragraph">
   Add related websites and links to reviews.
  </div>
 </Template>
</SnCore:Title>
<div class="sncore_cancel">
 <asp:HyperLink ID="linkNew" runat="server" Text="&#187; Link a Site" />
</div>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="PlaceWebsites" ShowHeader="false"
   AllowCustomPaging="true" AutoGenerateColumns="false" CssClass="sncore_account_table" PageSize="5">
   <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next"
    PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:TemplateColumn ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center">
      <ItemTemplate>
       <img src='PlaceWebsitePictureThumbnail.aspx?id=<%# Eval("Id") %>' alt='<%# Renderer.Render(Eval("Name")) %>' />
      </ItemTemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <ItemTemplate>
       <div class="sncore_h3">
        <%# Renderer.GetLink(Renderer.Render(Eval("Url")), Renderer.Render(Eval("Name"))) %>
        <div class="sncore_link">
         <%# Renderer.GetLink(Renderer.Render(Eval("Url")), Renderer.Render(Eval("Url")), 32) %>
        </div>
       </div>
       <div class="sncore_description">
        <%# base.Render(Eval("Description")) %>
       </div>
       <div class="sncore_link" style="text-align: right;">
        <asp:HyperLink id="linkEdit" runat="server" Visible='<%# CanWrite((int) Eval("AccountId")) %>' 
         NavigateUrl='<%# GetEditLink((int) Eval("Id")) %>' Text="&#187; edit" />
        <asp:LinkButton id="linkDelete" OnCommand="linkDelete_Command" runat="server" CommandName="Delete" 
         CommandArgument='<%# Eval("Id") %>' Text="&#187; delete" Visible='<%# CanWrite((int) Eval("AccountId")) %>' 
         OnClientClick='return confirm("Are you sure you want to delete this website?");' />
       </div>
     </ItemTemplate>
    </asp:TemplateColumn>
   </Columns>
  </SnCoreWebControls:PagedGrid>
 </ContentTemplate>
</asp:UpdatePanel>
