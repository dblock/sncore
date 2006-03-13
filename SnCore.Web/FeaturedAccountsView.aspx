<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="FeaturedAccountsView.aspx.cs" Inherits="FeaturedAccountsView" Title="Featured Accounts" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Featured People
    </div>
    <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
   </td>
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="FeaturedAccountsRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="FeaturedAccountsRss.aspx" />
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
     <a href="AccountView.aspx?id=<%# Eval("DataRowId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# base.GetAccount((int) Eval("DataRowId")).PictureId %>" />
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <div>
      <a class="sncore_account_name" href="AccountView.aspx?id=<%# Eval("DataRowId") %>">
       <%# base.Render(base.GetAccount((int) Eval("DataRowId")).Name) %>
      </a>
     </div>     
     <div class="sncore_description">
      featured on <%# ((DateTime) Eval("Created")).ToString("d") %>
     </div>
     <div class="sncore_description">
      <%# GetSummary(base.GetDescription((int) Eval("DataRowId"))) %>
     </div>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
