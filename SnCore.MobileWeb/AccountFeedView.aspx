<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFeedView.aspx.cs" Inherits="AccountFeedView" Title="Feed" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_links">
  <asp:Label ID="feedDescription" runat="server" />
 </div>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="5"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false" BorderWidth="0" BorderColor="White">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" PageButtonCount="5" />
  <ItemStyle CssClass="sncore_table_tr_td" />
  <HeaderStyle CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <div>
      <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
       <%# Renderer.Render(GetValue((string) Eval("Title"), "Untitled")) %>
      </a>
     </div>
     <div class="sncore_description">
      <a target="_blank" href='<%# Renderer.Render(Eval("Link")) %>'>
       &#187; x-posted
      </a>
      in
      <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
       <%# Renderer.Render((string) Eval("AccountName")) %>
      </a>'s
      <a href='AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>'>
       <%# Renderer.Render(GetValue((string) Eval("AccountFeedName"), "Untitled")) %>
      </a>
      <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
       &#187; <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>            
      </span>
     </div>
     <div>
      <%# GetDescription((string) Eval("Description"), (string) Eval("AccountFeedLinkUrl")) %>
     </div>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
