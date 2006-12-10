<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="FeaturedPlacesView.aspx.cs" Inherits="FeaturedPlacesView" Title="Featured Places" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Featured Places
    </div>
    <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
   </td>
   <td align="right" valign="middle">
    <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="FeaturedPlacesRss.aspx" />
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="10"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-Width="200px">
    <itemtemplate>
     <a href="PlaceView.aspx?id=<%# Eval("DataRowId") %>">
      <img border="0" src="PlacePictureThumbnail.aspx?id=<%# base.GetPlace((int) Eval("DataRowId")).PictureId %>" />
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <div>
      <a class="sncore_place_name" href="PlaceView.aspx?id=<%# Eval("DataRowId") %>">
       <%# base.Render(base.GetPlace((int) Eval("DataRowId")).Name) %>
      </a>
     </div>     
     <div class="sncore_description">
      featured on <%# ((DateTime) Eval("Created")).ToString("d") %>
     </div>
     <div class="sncore_description">
      <%# base.GetSummary(base.GetPlace((int) Eval("DataRowId")).Description) %>
     </div>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
