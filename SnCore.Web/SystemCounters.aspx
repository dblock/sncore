<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemCounters.aspx.cs"
 Inherits="SystemCounters" Title="Counters" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Counters
 </div>
 <asp:UpdatePanel runat="server" UpdateMode="Always" ID="panelCounters">
  <ContentTemplate>
   <div class="sncore_h2sub">
    <asp:HyperLink ID="linkHitStats" runat="server" Text="&#187; Hit Stats" NavigateUrl="SystemStatsHits.aspx" />
   </div>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
    AllowPaging="true" AutoGenerateColumns="false" AllowCustomPaging="True" CssClass="sncore_account_table">
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
    <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
      <itemtemplate>
       <img src='<%# ((DateTime) Eval("Created") > DateTime.UtcNow.AddDays(-7)) ? "images/account/star.gif" : "images/item.gif" %>' />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Total">
      <itemtemplate>
       <%# base.Render(Eval("Total")) %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Uri">
      <itemtemplate>
       <a target="_blank" href='<%# base.Render(Eval("Uri")) %>'>
        <%# GetUri((string) Eval("Uri")) %>
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Since">
      <itemtemplate>
       <%# SessionManager.ToAdjustedString((DateTime)Eval("Created"))%>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
