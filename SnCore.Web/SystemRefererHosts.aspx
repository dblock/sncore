<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemRefererHosts.aspx.cs"
 Inherits="SystemRefererHosts" Title="Referer Hosts" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Referer Hosts
 </div>
 <asp:UpdatePanel runat="server" UpdateMode="Always" ID="panelReferers">
  <ContentTemplate>
   <div class="sncore_h2sub">
    <asp:HyperLink ID="linkRefererCounters" runat="server" Text="&#187; Hit Stats" NavigateUrl="SystemStatsHits.aspx" />
    <asp:HyperLink ID="linkRefererHosts" runat="server" Text="&#187; Referer Hosts" Enabled="False" />
    <asp:HyperLink ID="linkRefererQueries" runat="server" Text="&#187; Referer Queries" NavigateUrl="SystemRefererQueries.aspx" />
   </div>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
    AllowPaging="true" AutoGenerateColumns="false" AllowCustomPaging="true" CssClass="sncore_account_table">
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
     <asp:TemplateColumn HeaderText="Host">
      <itemtemplate>
       <%# Renderer.GetLink(Renderer.Render(Eval("LastRefererUri")), Renderer.Render(Eval("Host"))) %>
       <div id='divLink' class="sncore_description" runat="server" visible='<%# (int) Eval("AccountId") > 0 %>'>
        <%# Renderer.GetLink(string.Format("AccountView.aspx?id={0}", Eval("AccountId")), Renderer.Render(Eval("AccountName"))) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <%# Renderer.GetLink(Renderer.Render(Eval("LastRefererUri")), "Link") %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href='SystemRefererAccountEdit.aspx?host=<%# Renderer.UrlEncode(Eval("Host")) %>'>
        <%# ((int) Eval("AccountId") == 0 ? "Add" : string.Empty) %>
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
